using UnityEngine;

// Starts a FullScreenUI dialogue when the Player enters a 2D trigger area.
public class FullScreenAreaTrigger : MonoBehaviour
{
    [SerializeField] private string dialogueName = "End";
    [SerializeField] private bool triggerOnce = true;

    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Trigger();
    }

    public void Trigger()
    {
        if (triggerOnce && hasTriggered)
            return;

        FullScreenUI fullScreenUI = FullScreenUI.Instance;

        if (fullScreenUI == null)
            fullScreenUI = FindObjectOfType<FullScreenUI>(true);

        if (fullScreenUI == null)
        {
            Debug.LogError($"{name}: FullScreenUI was not found.");
            return;
        }

        hasTriggered = true;
        fullScreenUI.Trigger(dialogueName);
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}
