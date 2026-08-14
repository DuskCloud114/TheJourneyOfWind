using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenUI : MonoBehaviour
{
    public static FullScreenUI Instance { get; private set; }

    [Header("Dialogue Data")]
    [SerializeField] private List<string> dialogueName = new List<string>();
    [SerializeField] private List<DialogueData> dialogueData = new List<DialogueData>();

    [Header("UI References")]
    [SerializeField] private Image fullScreenUI;
    [SerializeField] private TextMeshProUGUI fullScreenText;

    [Header("Playback")]
    [SerializeField] private float lineDuration = 2f;

    private readonly Dictionary<string, DialogueData> dialogueDict =
        new Dictionary<string, DialogueData>();

    private Coroutine playbackCoroutine;
    private string currentDialogueName;

    public bool IsImageActive { get; private set; }
    public bool IsTextActive { get; private set; }
    public bool IsTextOver { get; private set; }
    public bool IsBackgroundOpaque => fullScreenUI != null && fullScreenUI.color.a >= 0.999f;

    // The scene switcher subscribes to this event instead of being coupled to UI playback.
    public event Action<string> TextPlaybackCompleted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        BuildDialogueDictionary();

        if (fullScreenUI == null)
            Debug.LogError($"{name}: Full-screen Image is not assigned.");

        if (fullScreenText == null)
            Debug.LogError($"{name}: Full-screen TMP text is not assigned.");

        ResetUI();
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void BuildDialogueDictionary()
    {
        dialogueDict.Clear();

        if (dialogueName.Count != dialogueData.Count)
        {
            Debug.LogError($"{name}: dialogueName and dialogueData must have the same number of entries.");
            return;
        }

        for (int i = 0; i < dialogueName.Count; i++)
        {
            string key = dialogueName[i];

            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.LogError($"{name}: dialogueName contains an empty entry at index {i}.");
                continue;
            }

            if (dialogueData[i] == null)
            {
                Debug.LogError($"{name}: DialogueData for '{key}' is not assigned.");
                continue;
            }

            if (!dialogueDict.TryAdd(key, dialogueData[i]))
                Debug.LogError($"{name}: duplicate dialogue name '{key}'.");
        }
    }

    // UI-only entry point. It does not load a scene.
    public void Trigger(string diaName)
    {
        if (!dialogueDict.TryGetValue(diaName, out DialogueData data) || data == null)
        {
            Debug.LogError($"{name}: dialogue '{diaName}' was not found.");
            return;
        }

        if (data.dialogueLines == null || data.dialogueLines.Length == 0)
        {
            Debug.LogWarning($"{name}: dialogue '{diaName}' contains no lines.");
            return;
        }

        if (playbackCoroutine != null)
            StopCoroutine(playbackCoroutine);

        currentDialogueName = diaName;
        IsImageActive = true;
        IsTextActive = true;
        IsTextOver = false;

        SetImageAlpha(1f);
        fullScreenText.text = string.Empty;
        playbackCoroutine = StartCoroutine(PlayDialogue(data));
    }

    public void ResetUI()
    {
        if (playbackCoroutine != null)
        {
            StopCoroutine(playbackCoroutine);
            playbackCoroutine = null;
        }

        IsImageActive = false;
        IsTextActive = false;
        IsTextOver = false;
        currentDialogueName = string.Empty;

        SetImageAlpha(0f);

        if (fullScreenText != null)
            fullScreenText.text = string.Empty;
    }

    private IEnumerator PlayDialogue(DialogueData data)
    {
        float duration = Mathf.Max(0f, lineDuration);

        foreach (string line in data.dialogueLines)
        {
            fullScreenText.text = line;

            if (duration > 0f)
                yield return new WaitForSecondsRealtime(duration);
            else
                yield return null;
        }

        playbackCoroutine = null;
        IsTextOver = true;
        TextPlaybackCompleted?.Invoke(currentDialogueName);
    }

    private void SetImageAlpha(float alpha)
    {
        if (fullScreenUI == null)
            return;

        Color color = fullScreenUI.color;
        color.a = Mathf.Clamp01(alpha);
        fullScreenUI.color = color;
    }

    public void PlayBGM()
    {
        AudioManager.Instance?.PlayAudio("StartBGM", false);
    }
}
