using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;

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
        Hide();
    }

    void Start()
    {
        if (dialoguePanel == null) Debug.LogError("UIManager 未找到 dialoguePanel，请在 Inspector 中分配。");
        if (dialogueText == null) Debug.LogError("UIManager 未找到 dialogueText，请在 Inspector 中分配。");
    }

    public void Hide()
    {
        dialoguePanel.SetActive(false);
    }

    public void ShowDialogue(string text)
    {
        dialoguePanel.SetActive(true);
        dialogueText.text = text;
    }
}
