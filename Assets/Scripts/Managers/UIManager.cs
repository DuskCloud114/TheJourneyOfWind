using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("对话关闭")]
    [SerializeField] private float hideDelay = -1f; // 对话框隐藏延迟时间
    private Coroutine hideDialogueCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        HideDialogue();
        HidePrompt();
    }

    void Start()
    {
        if (hideDelay < 0) Debug.LogError("UIManager 的 hideDelay 未设置，请在 Inspector 中分配。"); 

        if (dialoguePanel == null) Debug.LogError("UIManager 未找到 dialoguePanel，请在 Inspector 中分配。");
        if (dialogueText == null) Debug.LogError("UIManager 未找到 dialogueText，请在 Inspector 中分配。");

        if (promptPanel == null) Debug.LogError("UIManager 未找到 promptPanel，请在 Inspector 中分配。");
        if (promptText == null) Debug.LogError("UIManager 未找到 promptText，请在 Inspector 中分配。");
    }

    public void HideDialogue()
    {
        // 离开范围时取消进行中的协程
        if (hideDialogueCoroutine != null)
        {
            StopCoroutine(hideDialogueCoroutine);
            hideDialogueCoroutine = null;
        }

        dialoguePanel.SetActive(false);
    }

    public void HidePrompt()
    {
        promptPanel.SetActive(false);
    }


    public void ShowDialogue(string text, bool isLast = false)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;

        if (isLast)
        {
            hideDialogueCoroutine = StartCoroutine(HideDialogueAfterDelay(hideDelay));
        }
    }
    public void ShowPrompt(string text)
    {
        promptPanel.SetActive(true);
        promptText.text = text;
    }

    IEnumerator HideDialogueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        HideDialogue();
        hideDialogueCoroutine = null;
    }
}
