using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class GlobalClass
{
    public static int CurrentLevel = 1;
    public static int CurrentCheckPoint = 0;

    public static List<Pickable> Inventory;
    public static List<string> Unlocks = new();

    public static void LoadLevel(int level, bool resetCheckpoint = false)
    {
        if (resetCheckpoint)
            CurrentCheckPoint = 0;

        SceneManager.LoadScene("Level" + level);
    }

    public static void ReloadLevel(bool resetCheckpoint = false)
    {
        if (resetCheckpoint)
            CurrentCheckPoint = 0;

        SceneManager.LoadScene("Level" + CurrentLevel);
    }
}
