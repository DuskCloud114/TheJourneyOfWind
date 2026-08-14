using UnityEngine;
using UnityEngine.SceneManagement;

// Listens for completion of a specific FullScreenUI dialogue and then changes scene or exits.
public class FullScreenSceneSwitcher : MonoBehaviour
{
    [SerializeField] private FullScreenUI fullScreenUI;
    [SerializeField] private string dialogueName = "Start";
    [SerializeField] private string targetSceneName = "FootOfHill";
    [SerializeField] private bool quitApplicationOnComplete;

    private bool isSwitching;

    private void OnEnable()
    {
        Subscribe();
    }

    private void Start()
    {
        
        Subscribe();
    }

    private void OnDisable()
    {
        if (fullScreenUI != null)
            fullScreenUI.TextPlaybackCompleted -= OnTextPlaybackCompleted;

        // Keep this callback alive while the old scene is being destroyed so the
        // persistent FullScreenUI can be reset after the new scene is loaded.
        if (!isSwitching)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Subscribe()
    {
        if (fullScreenUI == null)
            fullScreenUI = FullScreenUI.Instance;

        if (fullScreenUI != null)
        {
            fullScreenUI.TextPlaybackCompleted -= OnTextPlaybackCompleted;
            fullScreenUI.TextPlaybackCompleted += OnTextPlaybackCompleted;
        }
    }

    private void OnTextPlaybackCompleted(string completedDialogueName)
    {
        if (isSwitching || completedDialogueName != dialogueName)
            return;

        if (fullScreenUI == null || !fullScreenUI.IsBackgroundOpaque || !fullScreenUI.IsTextOver)
            return;

        if (quitApplicationOnComplete)
        {
            QuitApplication();
            return;
        }

        if (string.IsNullOrWhiteSpace(targetSceneName))
        {
            Debug.LogError($"{name}: target scene name is empty.");
            return;
        }

        isSwitching = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadSceneAsync(targetSceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        isSwitching = false;

        if (fullScreenUI != null)
            fullScreenUI.ResetUI();
    }

    private static void QuitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
