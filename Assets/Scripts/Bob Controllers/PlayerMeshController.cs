using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMeshController : MonoBehaviour
{
    public GameObject AliveParts, DeathParts;
    public float rotationLimit = 5f;

    GameObject bob;
    Rigidbody2D bobRb;

    private void Awake()
    {
        DeathParts.SetActive(true);
    }

    private void Start()
    {
        bob = GameObject.FindGameObjectWithTag("Player");
        bobRb = bob.GetComponent<Rigidbody2D>();

        foreach (SpriteRenderer s in DeathParts.GetComponentsInChildren<SpriteRenderer>())
        {
            s.enabled = false;
        }
    }

    bool deathPreapered = false;
    void Update()
    {
        if (bob != null)
        {
            HandlePosition();
        }
        else
        {
            if (deathPreapered)
                return;

            HandleDeath();
        }
    }

    void HandlePosition()
    {
        transform.position = bob.transform.position;

        var angles = transform.eulerAngles;
        var newZ = -bobRb.linearVelocity.x * 0.2f;
        if (Mathf.Abs(newZ) > rotationLimit)
            newZ = newZ > 0 ? rotationLimit : -rotationLimit;
        angles.z += newZ;

        transform.eulerAngles = angles;
    }

    void HandleDeath()
    {
        StartCoroutine(DeathRespawn());

        foreach (SpriteRenderer s in AliveParts.GetComponentsInChildren<SpriteRenderer>())
        {
            s.enabled = false;
        }

        foreach (SpriteRenderer s in DeathParts.GetComponentsInChildren<SpriteRenderer>())
        {
            s.enabled = true;
        }

        foreach (Rigidbody2D rb in DeathParts.GetComponentsInChildren<Rigidbody2D>())
        {
            rb.simulated = true;

            // Randomly push the ball in a random direction
            float randomPushDirection = Random.Range(-1f, 1f);  // Left or right
            float randomPushForce = Random.Range(5f, 20f);
            rb.AddForce(new Vector2(randomPushDirection * randomPushForce, 0f), ForceMode2D.Impulse);

            // Randomly apply torque for spinning
            float randomSpin = Random.Range(5f, 15f);
            rb.AddTorque(randomSpin, ForceMode2D.Impulse);

        }
        deathPreapered = true;
    }

    public IEnumerator DeathRespawn()
    {
        yield return new WaitForSeconds(1.2f);
        GlobalClass.ReloadLevel();
    }
}
