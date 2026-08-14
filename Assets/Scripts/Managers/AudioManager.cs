using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private List<string> audioName = new List<string>();
    [SerializeField] private List<AudioSource> audioSource = new List<AudioSource>();
    [SerializeField] private List<AudioClip> audioClip = new List<AudioClip>();

    private readonly Dictionary<string, AudioClip> audioDict = new Dictionary<string, AudioClip>();
    private readonly Dictionary<string, AudioSource> loopingSources = new Dictionary<string, AudioSource>();
    private AudioSource oneShotSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        BuildAudioDictionary();
        EnsureOneShotSource();
    }

    private void BuildAudioDictionary()
    {
        audioDict.Clear();
        if (audioName.Count != audioClip.Count)
        {
            Debug.LogError($"{name} audioName and audioClip must have the same number of entries.");
        }

        int count = Mathf.Min(audioName.Count, audioClip.Count);
        for (int i = 0; i < count; i++)
        {
            string key = audioName[i];
            if (string.IsNullOrWhiteSpace(key) || audioClip[i] == null) continue;
            if (audioDict.ContainsKey(key))
            {
                Debug.LogError($"Duplicate audio name '{key}' on {name}.");
            }
            else
            {
                audioDict.Add(key, audioClip[i]);
            }
        }
    }

    /// <summary>Play a configured clip by name. Looping clips keep one dedicated source.</summary>
    public void PlayAudio(string clipName, bool loop)
    {
        if (!audioDict.TryGetValue(clipName, out AudioClip clip))
        {
            Debug.LogWarning($"Audio clip '{clipName}' was not found on {name}.");
            return;
        }

        if (loop)
        {
            if (!loopingSources.TryGetValue(clipName, out AudioSource source))
            {
                source = CreateSource($"Loop_{clipName}");
                loopingSources.Add(clipName, source);
            }

            if (source.clip != clip) source.clip = clip;
            source.loop = true;
            if (!source.isPlaying) source.Play();
            return;
        }

        EnsureOneShotSource();
        oneShotSource.PlayOneShot(clip);
    }

    public void StopAudio(string clipName)
    {
        if (loopingSources.TryGetValue(clipName, out AudioSource source)) source.Stop();
    }

    private void EnsureOneShotSource()
    {
        if (oneShotSource != null) return;
        oneShotSource = CreateSource("OneShot");
    }

    private AudioSource CreateSource(string sourceName)
    {
        GameObject sourceObject = new GameObject(sourceName);
        sourceObject.transform.SetParent(transform, false);
        AudioSource source = sourceObject.AddComponent<AudioSource>();
        source.playOnAwake = false;
        audioSource.Add(source);
        return source;
    }
}
