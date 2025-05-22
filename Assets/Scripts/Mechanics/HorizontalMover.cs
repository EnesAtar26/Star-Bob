using UnityEngine;

public class HorizontalMover : MonoBehaviour
{
    public float leftLimit = -4f;
    public float rightLimit = 4f;
    public float speed = 2f;

    private bool movingRight = true;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            if (transform.position.x >= startPos.x + rightLimit)
                movingRight = false;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            if (transform.position.x <= startPos.x + leftLimit)
                movingRight = true;
        }
    }
}
