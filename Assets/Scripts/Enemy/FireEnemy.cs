using UnityEngine;

public class FireEnemy : MonoBehaviour
{
    public GameObject deathEffect;
    public GameObject fireballPrefab;
    public GameObject boxPrefab;
    public Transform firePoint;

    public float fireCooldown = 2f; // Ateþ etme aralýðý
    public float detectionRange = 10f; // Oyuncuyu fark etme mesafesi

    private float lastFireTime;
    private Transform player;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player != null && Vector2.Distance(transform.position, player.position) < detectionRange)
        {
            if (Time.time - lastFireTime > fireCooldown)
            {
                FireAtPlayer();
                lastFireTime = Time.time;
            }
        }
    }

    void FireAtPlayer()
    {
        Vector2 direction = (player.position - firePoint.position).normalized;

        GameObject fireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);

        // Fireball'un yönünü döndür
        fireball.transform.right = direction;

        // Hareket yönünü baþlat
        fireball.GetComponent<FireBallMovement>().Launch(direction);
    }

    public void Die()
    {
        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        CancelInvoke(); // Eðer Invoke kullanýlýyorsa
    }
   

    //public void TurnIntoBox()
    //{
      //  Instantiate(boxPrefab, transform.position, Quaternion.identity);
       // Destroy(gameObject);
    //}

}
