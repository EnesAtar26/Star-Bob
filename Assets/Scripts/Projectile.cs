using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public float rotationSpeed = 600f;
    public float moveSpeed = 3f;
    public float timer = 5f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            DestroyProjectile();
            return;
        }

        rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);

        Vector2 newPosition = rb.position + Vector2.right * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    void DestroyProjectile()
    {
        var mm = FindAnyObjectByType<MainManager>();
        Instantiate(mm.Projectile_Destroy).transform.position = transform.position;
        var pc = FindAnyObjectByType<PlayerController>();
        if (pc != null && pc.projectileCount > 0)
            pc.projectileCount--;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") ||
            collision.gameObject.layer == LayerMask.NameToLayer("nonGround"))
        {
            DestroyProjectile();
        }

        if (collision.tag == "Enemy")
        {
            collision.GetComponent<IEnemy>().Die();
            DestroyProjectile();
        }
    }
}