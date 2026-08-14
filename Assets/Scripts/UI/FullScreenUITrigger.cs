using UnityEngine;
using UnityEngine.UI;

// Put this component on a Start-scene Button. It only starts UI playback.
public class FullScreenUITrigger : MonoBehaviour
{
    [SerializeField] private string dialogueName = "Start";
    [SerializeField] private CanvasGroup buttonCanvasGroup;
    [SerializeField] private Button button;

    private void Awake()
    {
        if (buttonCanvasGroup == null)
            buttonCanvasGroup = GetComponent<CanvasGroup>();

        if (buttonCanvasGroup == null)
            buttonCanvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (button == null)
            button = GetComponent<Button>();
    }

    public void Trigger()
    {
        Trigger(dialogueName);
    }

    public void Trigger(string requestedDialogueName)
    {
        FullScreenUI fullScreenUI = FullScreenUI.Instance;

        if (fullScreenUI == null)
            fullScreenUI = FindObjectOfType<FullScreenUI>(true);

        if (fullScreenUI == null)
        {
            Debug.LogError("FullScreenUITrigger: FullScreenUI.Instance was not found.");
            return;
        }

        fullScreenUI.Trigger(requestedDialogueName);
    }

    // Bind this method explicitly in Button.onClick when the Button should disappear.
    public void HideButton()
    {
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.alpha = 0f;
            buttonCanvasGroup.interactable = false;
            buttonCanvasGroup.blocksRaycasts = false;
        }
        if (button != null)
            button.interactable = false;
    }

    public void ResetButton()
    {
        if (buttonCanvasGroup != null)
        {
            buttonCanvasGroup.alpha = 1f;
            buttonCanvasGroup.interactable = true;
            buttonCanvasGroup.blocksRaycasts = true;
        }

        if (button != null)
            button.interactable = true;
    }
}
