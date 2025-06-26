using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    public GameObject Main, Credits, Levels;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        Main.SetActive(false);
        Credits.SetActive(true);
        Levels.SetActive(false);
    }

    public void ShowMain()
    {
        Main.SetActive(true);
        Credits.SetActive(false);
        Levels.SetActive(false);
    }

    public void ShowLevels()
    {
        Main.SetActive(false);
        Credits.SetActive(false);
        Levels.SetActive(true);
    }

    public void SelectLevel(int i)
    {
        GlobalClass.LoadLevel(i, true);
    }
}
