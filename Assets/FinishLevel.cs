using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncuya deðdiyse
        {
            int nextLevel = GlobalClass.CurrentLevel + 1;
            GlobalClass.LoadLevel(nextLevel, resetCheckpoint: true);
        }
    }
}
