using System.Threading;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool isEnabled = true;
    public float speed = 3f;
    public Transform Checkpoint1, Checkpoint2;

    Rigidbody2D rb;
    Vector3 Check1, Check2;
    bool isMovingToCheckpoint1 = true;
    float checkPointsDistance;
    float p;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Check1 = Checkpoint1.position;
        Check2 = Checkpoint2.position;
        checkPointsDistance = Mathf.Abs((Checkpoint1.position - Checkpoint2.position).magnitude);
        Destroy(Checkpoint1.gameObject);
        Destroy(Checkpoint2.gameObject);
        p = (1f - Mathf.Abs((Check1 - transform.position).magnitude) / checkPointsDistance);
    }

    private void Update()
    {
        if (!isEnabled) return;

        switch (isMovingToCheckpoint1)
        {
            case true:
                p += (speed / 10f) * Time.deltaTime;
                if (p < 0.99f)
                    rb.MovePosition(Vector2.LerpUnclamped(Check2, Check1, p));
                else
                {
                    rb.MovePosition(Check1);
                    isMovingToCheckpoint1 = false;
                    p = 0f;
                }
                break;

            case false:
                p += (speed / 10f) * Time.deltaTime;
                if (p < 0.99f)
                    rb.MovePosition(Vector2.LerpUnclamped(Check1, Check2, p));
                else
                {
                    rb.MovePosition(Check2);
                    isMovingToCheckpoint1 = true;
                    p = 0f;
                }
                break;
        }
    }
}
