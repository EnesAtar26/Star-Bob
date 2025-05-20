using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public ElevatorScript elevatorScript;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            elevatorScript.PlayerGotKey();
            Destroy(gameObject);
        }
    }
}
