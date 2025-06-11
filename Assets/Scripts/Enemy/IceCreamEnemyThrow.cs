using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public GameObject iceCreamPrefab;
    public Transform player;
    public Transform firePoint; // Atış noktası
    public float attackRange = 10f;
    public float cooldownTime = 7f;
    public float baseForce = 5f;
    public float initialDelay = 1.5f;
    private bool isAttacking = false;

    void Update()
    {
        FlipTowardsPlayer();
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange && !isAttacking)
        {
            StartCoroutine(FireIceCreams());
        }
    }

    IEnumerator FireIceCreams()
    {
        isAttacking = true;
        yield return new WaitForSeconds(initialDelay);

        for (int i = 1; i <= 3; i++)
        {
            FireSingleIceCream(i);
            yield return new WaitForSeconds(1f); // her atış arasında 1 saniye bekle
        }

        yield return new WaitForSeconds(cooldownTime);
        isAttacking = false;
    }


    void FireSingleIceCream(int multiplier)
    {
        Vector2 direction = (player.position - transform.position).normalized;
        direction.y = 0f; // sadece yatay yön
        direction = direction.normalized;

        float force = baseForce * multiplier;

        GameObject iceCream = Instantiate(iceCreamPrefab, firePoint.position, Quaternion.identity);

        // Yön belirleme (sağda mı, solda mı)
        bool isPlayerOnRight = player.position.x > transform.position.x;

        // Sprite'ın yönünü çevir (ölçekleme ile)
        Vector3 scale = iceCream.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isPlayerOnRight ? 1 : -1);
        iceCream.transform.localScale = scale;

        // Rigidbody ile kuvvet uygulama
        Rigidbody2D rb = iceCream.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero; // önceki hareketi sıfırla
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }
    }



void FlipTowardsPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        bool isPlayerOnLeft = player.position.x < transform.position.x;

        // Sprite sağa bakıyorsa scale.x pozitif olmalı
        if (isPlayerOnLeft)
        {
            scale.x = -Mathf.Abs(scale.x); // sola bak
        }
        else
        {
            scale.x = Mathf.Abs(scale.x); // sağa bak
        }

        transform.localScale = scale;

       
        
    }
}
