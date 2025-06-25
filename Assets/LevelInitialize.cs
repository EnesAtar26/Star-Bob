using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelInitialize : MonoBehaviour
{
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName.StartsWith("Level"))
        {
            string numberPart = sceneName.Substring("Level".Length);
            if (int.TryParse(numberPart, out int levelNumber))
            {
                GlobalClass.CurrentLevel = levelNumber;
            }
        }
    }
}
