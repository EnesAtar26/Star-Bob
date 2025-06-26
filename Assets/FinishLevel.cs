using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Oyuncuya deðdiyse
        {
            int nextLevel = GlobalClass.CurrentLevel + 1;
            if (nextLevel == 31) SceneManager.LoadScene("Menu");
            if (nextLevel == 4) nextLevel = 30;

            GlobalClass.LoadLevel(nextLevel, resetCheckpoint: true);
        }
    }
}
