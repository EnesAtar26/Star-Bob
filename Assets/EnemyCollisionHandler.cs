
using System;
using UnityEngine;


public class EnemyCollisionHandler : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killer"))
        {
            // Oyuncunun pozisyonu düþmandan yukarýdaysa ve gerçekten yukarýdan çarpmýþsa
            Vector2 contactPoint = collision.GetContact(0).point;
            Vector2 enemyPosition = collision.gameObject.transform.position;

            if (contactPoint.y > enemyPosition.y + 0.3f) // 0.3 deðeri düþmanýn yüksekliðine göre ayarlanabilir
            {
                Collider2D enemyCollider = collision.collider;
                enemyCollider.isTrigger = false;

                // Üstten çarptý ? düþman ölsün
                Destroy(collision.gameObject);

                // Oyuncuyu biraz zýplat (isteðe baðlý)
               
            }

        }
    }


}
