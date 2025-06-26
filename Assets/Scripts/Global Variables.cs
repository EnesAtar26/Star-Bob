using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GlobalClass
{
    public static int CurrentLevel;
    public static int CurrentCheckPoint = 0;

    public static float CurrentTimeSeconds = 0f;
    public static int Score = 0;

    public static float MusicLeftover = 0f;

    public static List<Pickable> Inventory;
    public static List<string> Unlocks = new();

    public static void LoadLevel(int level, bool resetCheckpoint = false)
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (resetCheckpoint)
            CurrentCheckPoint = 0;

        CurrentLevel = level;
        CurrentCheckPoint = 0;
        CurrentTimeSeconds = 0f;
        MusicLeftover = 0f;
        SceneManager.LoadScene("Level" + level);
    }

    public static void ReloadLevel(bool resetCheckpoint = false)
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (resetCheckpoint)
            CurrentCheckPoint = 0;

        SceneManager.LoadScene("Level" + CurrentLevel);
    }
}
