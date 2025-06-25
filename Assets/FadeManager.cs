using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Fade objesi sahne deðiþiminde kaybolmasýn
    }

    public void FadeToScene(int sceneIndex)
    {
        StartCoroutine(FadeAndSwitchScenes(sceneIndex));
    }

    private IEnumerator FadeAndSwitchScenes(int sceneIndex)
    {
        yield return StartCoroutine(Fade(1)); // Ekraný karart
        SceneManager.LoadScene(sceneIndex);
        yield return StartCoroutine(Fade(0)); // Yeni sahnede açýl
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float alpha = fadeImage.color.a;
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float t = timer / fadeDuration;
            float newAlpha = Mathf.Lerp(alpha, targetAlpha, t);
            fadeImage.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
    }

    public void FadeToSceneByLevelNumber(int level)
    {
        string sceneName = "Level" + level;
        StartCoroutine(FadeAndSwitchScenes(sceneName));
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1)); // Karart
        SceneManager.LoadScene(sceneName);
        yield return StartCoroutine(Fade(0)); // Aç
    }
}