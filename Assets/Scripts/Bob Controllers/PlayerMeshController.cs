using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMeshController : MonoBehaviour
{
    public GameObject AliveParts, DeathParts;
    public GameObject NormalPart, ArmorPart, PizzaPart, IceCreamPart;
    public float rotationLimit = 5f;

    public AudioSource audioSource;

    GameObject bob;
    Rigidbody2D bobRb;
    PlayerController playerController;

    private void Awake()
    {
        DeathParts.SetActive(true);
    }

    private void Start()
    {
        bob = GameObject.FindGameObjectWithTag("Player");
        bobRb = bob.GetComponent<Rigidbody2D>();
        playerController = FindAnyObjectByType<PlayerController>();

        UpdatePlayerMeshImprovement();

        foreach (SpriteRenderer s in DeathParts.GetComponentsInChildren<SpriteRenderer>())
        {
            s.enabled = false;
        }
    }

    public void UpdatePlayerMeshImprovement()
    {
        switch (playerController.Improvement)
        {
            case BobImprovements.None:
                NormalPart.SetActive(true);
                ArmorPart.SetActive(false);
                PizzaPart.SetActive(false);
                IceCreamPart.SetActive(false);
                WaterPowerPart.SetActive(false);
                break;

            case BobImprovements.Armor:
                NormalPart.SetActive(false);
                ArmorPart.SetActive(true);
                PizzaPart.SetActive(false);
                IceCreamPart.SetActive(false);
                WaterPowerPart.SetActive(false);
                break;

            case BobImprovements.Pizza:
                NormalPart.SetActive(false);
                ArmorPart.SetActive(false);
                PizzaPart.SetActive(true);
                IceCreamPart.SetActive(false);
                WaterPowerPart.SetActive(false);
                break;

            case BobImprovements.IceCream:
                NormalPart.SetActive(false);
                ArmorPart.SetActive(false);
                PizzaPart.SetActive(false);
                IceCreamPart.SetActive(true);
                WaterPowerPart.SetActive(false);
                break;
            case BobImprovements.WaterDrop:
                NormalPart.SetActive(false);
                ArmorPart.SetActive(false);
                PizzaPart.SetActive(false);
                IceCreamPart.SetActive(false);
                WaterPowerPart.SetActive(true);
                break;

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
        GlobalClass.MusicLeftover = FindAnyObjectByType<MainManager>().GetComponent<AudioSource>().time;
        GlobalClass.ReloadLevel();
    }
}
