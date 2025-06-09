using System.Collections;
using UnityEngine;

public class EnemyThrow : MonoBehaviour
{
    public Transform rightArm;
    public GameObject iceCreamPrefab;
    public Transform throwPoint;
    public Transform player; // Oyuncuyu buraya atayacaðýz
    public float throwAngle = -45f;
    public float fireRange = 5f; // Kaç birim yakýnlýkta fýrlatsýn?
    public float fireCooldown = 2f; // 2 saniyede bir fýrlatsýn
    private float lastFireTime;

    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= fireRange && Time.time >= lastFireTime + fireCooldown)
        {
            FireIceCream();
            lastFireTime = Time.time;
        }
    }

    public void FireIceCream()
    {
        rightArm.localRotation = Quaternion.Euler(0, 0, throwAngle);

        // Dondurmayý instantiate et
        GameObject iceCream = Instantiate(iceCreamPrefab, throwPoint.position, Quaternion.identity);

        // Rigidbody2D bileþenini al
        Rigidbody2D rb = iceCream.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            // Dondurmanýn karaktere doðru yönünü hesapla
            Vector2 direction = (player.position - throwPoint.position).normalized;

            // Hýzýný ayarla (örnek hýz 10f)
            float speed = 10f;
            rb.linearVelocity = direction * speed;
        }

        StartCoroutine(ResetArm());
    }


    IEnumerator ResetArm()
    {
        yield return new WaitForSeconds(0.2f);
        rightArm.localRotation = Quaternion.Euler(0, 0, 0);
    }
}
