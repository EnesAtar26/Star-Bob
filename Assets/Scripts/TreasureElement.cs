using System.Globalization;
using UnityEngine;

public class TreasureElement : MonoBehaviour
{
    public void Initilize(PickableType Type)
    {
        type = Type;
        startPosition = transform.localPosition;
        start = true;
        Bob = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        mainManager = FindAnyObjectByType<MainManager>();
    }

    public float moveSpeed = 3f;

    PickableType type;
    bool start = false;
    float timer = 0f;
    Vector3 startPosition = Vector3.zero;
    int state = 0;
    GameObject Bob;
    Rigidbody2D rb;
    MainManager mainManager;


    private void FixedUpdate()
    {
        if (!start) return;


        switch (type)
        {
            case PickableType.ConeIceCream:
                if (Appear())
                    Destroy(gameObject);
                timer += Time.deltaTime;
                break;

            case PickableType.BowlIceCream:
                if (Appear(0.9f, 1f))
                    Destroy(this);
                timer += Time.deltaTime;
                break;

            case PickableType.Donut:
                switch (state)
                {
                    case 0:
                        if (Appear(0.8f, 1f))
                        {
                            rb.simulated = true;
                            state = 1;
                            if (Bob != null && transform.position.x < Bob.transform.position.x)
                            {
                                moveSpeed *= -1f;
                            }
                        }
                        timer += Time.deltaTime;
                        break;

                    case 1:
                        CheckColliders_Turn();
                        rb.linearVelocityX = moveSpeed;
                        break;
                }
                break;

            case PickableType.Cupcake:
                switch (state)
                {
                    case 0:
                        if (Appear(0.8f, 1f))
                        {
                            rb.simulated = true;
                            state = 1;
                            if (Bob != null && transform.position.x < Bob.transform.position.x)
                            {
                                moveSpeed *= -1f;
                            }
                        }
                        timer += Time.deltaTime;
                        break;

                    case 1:
                        CheckColliders_Jump();
                        CheckColliders_Turn();
                        rb.linearVelocityX = moveSpeed;
                        break;
                }
                break;
        }
    }

    void CheckColliders_Jump()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.down, 0.4f, mainManager.TreausureFlipLayers);

        if (hitLeft.collider != null && hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.linearVelocityY = 6f;
        }
    }

    void CheckColliders_Turn()
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.4f, mainManager.TreausureFlipLayers);
        RaycastHit2D hitRight = Physics2D.Raycast(transform.position, Vector2.right, 0.4f, mainManager.TreausureFlipLayers);

        if (hitLeft.collider != null && hitLeft.collider.tag != "Ignore" &&
            hitLeft.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            moveSpeed *= -1;
        }

        if (hitRight.collider != null && hitRight.collider.tag != "Ignore" &&
            hitRight.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            moveSpeed *= -1;
        }
    }

    bool Appear(float maxY = 2.5f, float time = 0.5f)
    {
        var p = transform.localPosition;
        p.y = maxY;
        float i = timer / time;

        transform.localPosition = Vector3.Lerp(startPosition, p, i);

        if (timer > time)
            return true;
        return false;
    }
}
