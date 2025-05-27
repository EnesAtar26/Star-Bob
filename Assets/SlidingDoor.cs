using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public Vector2 openOffset;
    public float slideSpeed = 2f;

    private Vector2 initialPos;
    private Vector2 targetPos;
    private bool isOpen = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialPos.x = transform.position.x;
        targetPos.x = transform.position.x + openOffset.x;


    }

    void FixedUpdate()
    {
        if (isOpen)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPos,slideSpeed*Time.fixedDeltaTime));
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
}
