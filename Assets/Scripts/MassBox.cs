using UnityEngine;

public class MassBox : MonoBehaviour
{
    public GameObject TopBox;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlatformBox" && collision.transform.position.y > transform.position.y)
        {
            TopBox = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlatformBox")
        {
            TopBox = null;
        }
    }
}
