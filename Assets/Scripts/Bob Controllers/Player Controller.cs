using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Interfaces;
using UnityEngine;

public enum BobImprovements
{
    None,
    Armor,
    Pizza,
    IceCream,
    WaterDrop
}

public class PlayerController : MonoBehaviour
{
    [Header("States")]
    public BobImprovements Improvement;
    public List<Pickable> Inventory;
    public int IceCream = 0;
    public bool hasWaterPower = false;
    public bool isDead = false;
    public bool isInvicible = false;
    public bool isDeadly = false;
    public float invicibleTime = 0f;
    public int projectileCount = 0;
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
    public float standartDamp = 0f;   // Oyuncu topla ayn� y�nde hareket ederkenki s�rt�nme
    public float turnDamp = 3f;     // Oyuncu topla ters y�nde hareket ederkenki s�rt�nme (Sadece yerdeyken)
    public float groundFreeDamp = 2f;     // Oyuncu bir tu�a basmad���nda olan s�rt�nme
    public float torqueForce = 0f;
    public float maxSpeed = 8f;

    private Rigidbody2D rb;
    private float moveInput;
    private bool isGrounded;

    private PlayerMeshController meshController;
    private MainManager mainManager;
    private GameObject invicibleEffect;

    private void Awake()
    {
        mainManager = FindAnyObjectByType<MainManager>();
        meshController = FindAnyObjectByType<PlayerMeshController>();

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

        foreach (Lock l in FindObjectsByType<Lock>(FindObjectsSortMode.None))
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

    void HandleProjectiles()
    {
        if (projectileCount > 1)
            return;

        var g = Instantiate(mainManager.Projectile_IceCream);
        var p = transform.position;
        p.x += 0.2f;
        p.y += 0.2f;
        g.transform.position = p;
        projectileCount++;

        var prj = g.GetComponent<Projectile>();
        if (rb.linearVelocityX < 0f)
            prj.moveSpeed *= -1f;
    }

    void HandleInput()
    {
        // Get horizontal input
        moveInput = Input.GetAxis("Horizontal");

        if (Improvement == BobImprovements.IceCream || Improvement == BobImprovements.Pizza)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                HandleProjectiles();
            }
        }

        //Restart
        if (Input.GetKey(KeyCode.R))
        {
            GlobalClass.ReloadLevel();
        }
    }

    void CheckInvicible()
    {
        if (invicibleTime > 0f)
        {
            isInvicible = true;
            invicibleTime -= Time.deltaTime;
        }
        else
        {
            if (invicibleEffect != null)
            {
                Destroy(invicibleEffect);
                invicibleEffect = null;
            }
            isInvicible = false;
            isDeadly = false;
        }
    }



    void Update()
    {
        CheckInvicible();
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
                if (isGrounded && Mathf.Sign(rb.linearVelocity.x) != Mathf.Sign(moveInput)) // Ters y�nde hareket
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

    void BobDeimprove()
    {
        switch (Improvement)
        {
            case BobImprovements.None:
                break;

            case BobImprovements.Armor:
                invicibleTime = 2f;
                Improvement = BobImprovements.None;
                var bh = Instantiate(mainManager.BrokenHeart);
                bh.transform.position = transform.position;
                break;

            default:
                invicibleTime = 2f;
                Improvement = BobImprovements.Armor;
                var bh2 = Instantiate(mainManager.BrokenHeart);
                bh2.transform.position = transform.position;
                break;
        }
        meshController.UpdatePlayerMeshImprovement();
    }

    void BobImprove(BobImprovements i)
    {
        if (Improvement == BobImprovements.None) // E�er Bob'un herhangi bir improvement'i yoksa sadece armor alabilir
            i = BobImprovements.Armor;

        switch (i)
        {
            case BobImprovements.Armor:
                Instantiate(mainManager.Poof, transform);
                break;

            case BobImprovements.IceCream:
                Instantiate(mainManager.Poof, transform);
                break;
            case BobImprovements.WaterDrop:
                Instantiate(mainManager.Poof, transform);
                break;
        }

        Improvement = i;
        meshController.UpdatePlayerMeshImprovement();
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

            case PickableType.Donut:
                BobImprove(BobImprovements.Armor);
                break;

            case PickableType.Cupcake:
                invicibleTime = 10f;
                isDeadly = true;
                if (invicibleEffect == null)
                    invicibleEffect = Instantiate(mainManager.Invicible, transform);
                break;

            case PickableType.BowlIceCream:
                BobImprove(BobImprovements.IceCream);
                break;
            
            case PickableType.WaterDrop:
                hasWaterPower = true;
                BobImprove(BobImprovements.WaterDrop);

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

    void EnemyCollision(Collider2D collision)
    {
        if (isDeadly)
        {
            collision.GetComponent<FireEnemy>().Die();
            var dirX = transform.position.x - collision.transform.position.x;
            float p = dirX < 0 ? 8f : -8f;
            rb.linearVelocity = new Vector2(rb.linearVelocityX + p, rb.linearVelocityY);
            return;
        }

        Vector2 contactPoint = collision.ClosestPoint(transform.position);
        float playerY = transform.position.y;
        float enemyY = collision.transform.position.y;


        if (playerY > enemyY + 0.5f && hasWaterPower == true)
        {

            collision.GetComponent<FireEnemy>().TurnIntoBox();
            GetComponent<Rigidbody2D>().linearVelocity = new Vector2(GetComponent<Rigidbody2D>().linearVelocity.x, 10f);
        }


        else
        {
            if (isInvicible)
                return;
            else if (Improvement != BobImprovements.None)
            {
                BobHit(collision.transform);
                BobDeimprove();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        }
    

        void BobHit(Transform t)
        {
            Vector2 dir = Vector2.zero;
            dir.x = transform.position.x < t.position.x ? -10f : 10f;
            dir.y = 7f;

            rb.linearVelocity = dir;
        }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Killer":
                if (!isInvicible)
                {
                    if (Improvement != BobImprovements.None)
                    {
                        BobHit(collision.transform);
                        BobDeimprove();
                    }
                    else
                        Destroy(gameObject);
                }
                break;

            case "Force Killer":
                Destroy(gameObject);
                break;

            case "Enemy":
                EnemyCollision(collision);
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