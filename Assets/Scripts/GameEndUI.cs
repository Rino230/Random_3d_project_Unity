using UnityEngine;
using System.Collections;

public class GameEndUI : MonoBehaviour
{
    public static bool GameEnded { get; private set; }

    [Header("End Game Panels")]
    [SerializeField] private CanvasGroup victoryPanel;
    [SerializeField] private CanvasGroup defeatPanel;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip victorySound;
    [SerializeField] private AudioClip defeatSound;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 1f;

    private bool gameEnded;

    private void Awake()
    {
        GameEnded = false;
        Time.timeScale = 1f;
    }

    public void ShowVictory()
    {
        if (gameEnded) return;

        gameEnded = true;
        GameEnded = true;

        if (victorySound != null)
            audioSource.PlayOneShot(victorySound);

        StartCoroutine(FadeIn(victoryPanel));
    }

    public void ShowDefeat()
    {
        if (gameEnded) return;

        gameEnded = true;
        GameEnded = true;

        if (defeatSound != null)
            audioSource.PlayOneShot(defeatSound);

        StartCoroutine(FadeIn(defeatPanel));
    }

    private IEnumerator FadeIn(CanvasGroup panel)
    {
        panel.gameObject.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            panel.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        panel.alpha = 1f;

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}