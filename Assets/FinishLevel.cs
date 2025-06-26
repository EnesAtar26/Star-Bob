using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncuya deðdiyse
        {
            int nextLevel = GlobalClass.CurrentLevel + 1;
            if (nextLevel == 4) nextLevel = 30;

            GlobalClass.MusicLeftover = 0f;
            GlobalClass.CurrentTimeSeconds = 0f;
            GlobalClass.LoadLevel(nextLevel, resetCheckpoint: true);
        }
    }
}
