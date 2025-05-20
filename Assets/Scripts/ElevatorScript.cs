using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    //public float topPos = -8f;
    //public float bottomPos = -17f;
    //public float speed = 2f;
    //private float height = 9;
    //private Vector3 startPos;

    private bool playerHasKey = false;
    private bool activateElevator = false;

    public float maxHeight = -10f;
    public float minHeight = -19f;
    public float speedUp = 2f;
    public float speedDown = 2f;
    private bool movingUp = false;
    private float step = 0f;
    private Rigidbody2D rb;

    /*void Awake()
    {
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        moveElevator();
    }

    private void moveElevator()
    {
        if (playerHasKey && activateElevator)
        {
            float newY = Mathf.PingPong(Time.fixedTime * speed, height) + bottomPos;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    }*/

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Vector2 currentPosition = rb.position;

        if (playerHasKey && activateElevator)
        {
            if (movingUp)
            {
                step = speedUp * Time.fixedDeltaTime;
                currentPosition.y += step;

                if (currentPosition.y >= maxHeight)
                {
                    currentPosition.y = maxHeight;
                    movingUp = false;
                }
            }
            else
            {
                step = speedUp * Time.fixedDeltaTime;
                currentPosition.y -= step;

                if (currentPosition.y <= minHeight)
                {
                    currentPosition.y = minHeight;
                    movingUp = true;
                }
            }

            rb.MovePosition(currentPosition);
        }

    }

    public void PlayerGotKey()
    {
        playerHasKey = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            activateElevator = true;
        }
    }
}
