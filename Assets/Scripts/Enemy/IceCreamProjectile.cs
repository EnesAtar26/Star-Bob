using UnityEngine;

public class IceCreamProjectile : MonoBehaviour
{
    public float lifeTime = 5f;
    private bool hitGround = false;

    private void Start()
    {
        // Ýstersen genel yaþam süresi de býrakabilirsin
        // Destroy(gameObject, lifeTime); // Bu satýrý kullanma istersen sadece yere göre yok olsun
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && !hitGround)
        {
            hitGround = true;
            

            // Rigidbody'yi dondur
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;
                rb.bodyType = RigidbodyType2D.Static; // Tamamen sabitle
            }

            // 5 saniye sonra yok et
            Destroy(gameObject, lifeTime);
        }
        else if (!other.CompareTag("Enemy") && !other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }

}
