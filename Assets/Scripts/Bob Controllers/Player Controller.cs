using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

public class PlayerController : MonoBehaviour
{
    [Header("States")]
    public List<Pickable> Inventory;
    public int IceCream = 0;
    public bool isDead = false;
    [Space]

    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public bool allowAirControl = true;

    [Header("Ground Check Settings")]
    public float groundCheckRadius = 0.253f;
    public LayerMask groundLayer;
    public bool visualize;

    [Header("Physics")]
    public float standartDamp = 0f;   // Oyuncu topla ayný yönde hareket ederkenki sürtünme
    public float turnDamp = 3f;     // Oyuncu topla ters yönde hareket ederkenki sürtünme (Sadece yerdeyken)
    public float groundFreeDamp = 2f;     // Oyuncu bir tuþa basmadýðýnda olan sürtünme
    public float torqueForce = 0f;
    public float maxSpeed = 8f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    private void Awake()
    {
        if (GlobalClass.Inventory == null)
            GlobalClass.Inventory = Inventory;
        else
            Inventory = GlobalClass.Inventory;

        foreach (CheckPoint c in FindObjectsByType<CheckPoint>(FindObjectsSortMode.None))
        {
            if (c.order == GlobalClass.CurrentCheckPoint)
            {
                c.Save();
                transform.position = c.spawn.position;
            }
            else if (c.order < GlobalClass.CurrentCheckPoint)
            {
                c.Save();
            }
        }

        foreach(Lock l in FindObjectsByType<Lock>(FindObjectsSortMode.None))
        {
            if (GlobalClass.Unlocks.Contains(l.Key))
                Destroy(l.DestroyRoot);
        }
        var f = FindObjectsByType<Pickable>(FindObjectsSortMode.None);
        foreach (Pickable p in FindObjectsByType<Pickable>(FindObjectsSortMode.None))
        {
            switch (p.Type)
            {
                case PickableType.ConeIceCream:
                    if (GlobalClass.Inventory.Any(x => x.Id == p.Id))
                        Destroy(p.DestroyRoot);
                    break;

                case PickableType.Other:
                    if (GlobalClass.Unlocks.Contains(p.Name))
                        Destroy(p.DestroyRoot);
                    break;
            }
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = false; // Allow ball rotation
    }

    void HandleInput()
    {
        // Get horizontal input
        moveInput = Input.GetAxis("Horizontal");
        
        //Restart
        if (Input.GetKey(KeyCode.R))
        {
            GlobalClass.ReloadLevel();
        }
    }

    void Update()
    {
        HandleInput();

        // Check for ground using raycast
        var p = transform.position;
        p.y -= 0.01f;
        RaycastHit2D hit = Physics2D.CircleCast(p, groundCheckRadius, Vector2.down, 0.1f, groundLayer);
        isGrounded = hit.collider != null;

        // Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Apply movement only if grounded or air control is allowed
        if (isGrounded || allowAirControl)
        {
            // Apply damping
            if (moveInput == 0 && isGrounded)
            {
                rb.linearDamping = groundFreeDamp;
            }
            else
            {
                if (isGrounded && Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(moveInput)) // Ters yönde hareket
                {
                    rb.linearDamping = turnDamp;
                }
                else
                {
                    rb.linearDamping = standartDamp;
                }
            }

            if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed)
            {
                rb.AddForce(new Vector2(moveInput * moveSpeed, 0f), ForceMode2D.Force);
            }

            // Add torque to roll the ball
            rb.AddTorque(-moveInput * torqueForce);
        }
        else // Havadayken
        {
            rb.linearDamping = standartDamp;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!visualize)
            return;

        // Visualize ground check
        if (isGrounded)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.position, groundCheckRadius);
        }

        Gizmos.color = Color.gray;
        var p = transform.position;
        p.y -= 0.01f;
        Gizmos.DrawWireSphere(p, 0.253f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundCheckRadius);
    }

    void Pickup(Pickable item)
    {
        switch (item.Type)
        {
            case PickableType.ConeIceCream:
                IceCream++;
                break;

            case PickableType.Other:
                Inventory.Add(item.Data());
                break;
        }
        Destroy(item.DestroyRoot);
    }

    void PickupGeneric(PickableType type)
    {
        switch (type)
        {
            case PickableType.ConeIceCream:
                IceCream++;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Killer":
                Destroy(gameObject);
                break;

            case "Enemy":
                Vector2 contactPoint = collision.ClosestPoint(transform.position);
                float playerY = transform.position.y;
                float enemyY = collision.transform.position.y;

               
                if (playerY > enemyY + 0.5f)
                {
                    collision.GetComponent<Enemy>().Die();
                    GetComponent<Rigidbody2D>().linearVelocity = new Vector2(GetComponent<Rigidbody2D>().linearVelocity.x, 10f);
                }
                else
                {
                    Destroy(gameObject); 
                }
                break;



            case "Pusher":
                var pusher = collision.GetComponent<Pusher>();
                Vector2 v = rb.linearVelocity;
                if (pusher.x) v.x = pusher.Power.x;
                if (pusher.y) v.y = pusher.Power.y;
                rb.linearVelocity = v;
                break;

            case "Pickable":
                Pickup(collision.GetComponent<Pickable>());
                break;

            case "Treasure":
                var t = collision.GetComponent<Treasure>();
                if (t.TickTreasure()) PickupGeneric(t.Type);
                break;
        }
    }
}
