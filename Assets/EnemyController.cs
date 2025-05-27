using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    public float hareketHizi = 2f;
    public float hareketMesafesi = 3f;
    public float donusHizi = 100f;
    public float gravityScale = 0.5f; // Hafif bir yerçekimi

    private Vector2 baslangicPozisyonu;
    private bool sagaGidiyor = true;
    private Rigidbody2D rb;

    void Start()
    {
        baslangicPozisyonu = transform.position;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        rb.freezeRotation = true; // Fiziksel rotasyonu dondur
    }

    void Update()
    {
        // Karakterin hareket yönüne göre dönüþ yönünü ayarla
        float donusYon = sagaGidiyor ? -1f : 1f;
        transform.Rotate(0f, 0f, donusYon * donusHizi * Time.deltaTime);
    }

    void FixedUpdate()
    {
        float hareketYonu = sagaGidiyor ? 1 : -1;
        Vector2 hareket = new Vector2(hareketYonu * hareketHizi, rb.linearVelocity.y);
        rb.linearVelocity = hareket;

        if (sagaGidiyor && transform.position.x >= baslangicPozisyonu.x + hareketMesafesi)
        {
            sagaGidiyor = false;
        }
        else if (!sagaGidiyor && transform.position.x <= baslangicPozisyonu.x - hareketMesafesi)
        {
            sagaGidiyor = true;
        }

        // Zemin kontrolü ve yere bastýrma
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
        if (!raycastHit.collider)
        {
            // Eðer zemin yoksa, aþaðýya doðru hafif bir kuvvet uygula
            rb.AddForce(Vector2.down * 5f);
        }
    }
}
