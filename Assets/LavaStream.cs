using UnityEngine;

public class LavaStream : MonoBehaviour
{
    public float streamSpeed = 0.5f; // Lavýn nesneyi sürüklediði hýz
    public Vector2 streamDirection = Vector2.right; // Saða sürüklüyor

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Yavaþ ama sabit bir yatay hareket uygula (hýz ekle deðil, doðrudan uygula)
            Vector2 velocity = rb.linearVelocity;
            velocity.x = streamDirection.normalized.x * streamSpeed;
            rb.linearVelocity = velocity;
        }
    }
}
