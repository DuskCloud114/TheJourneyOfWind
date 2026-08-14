using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWindow : MonoBehaviour
{
    [Header("Build Window")]
    [Range(0.1f, 1f)]
    [SerializeField] private float buildWindowScale = 0.57f;
    
    void Start()
    {
        SetBuildWindowSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SetBuildWindowSize()
    {
#if !UNITY_EDITOR
        if (buildWindowScale <= 0f) return;

        int width = Mathf.Max(1, Mathf.RoundToInt(Display.main.systemWidth * buildWindowScale));
        int height = Mathf.Max(1, Mathf.RoundToInt(Display.main.systemHeight * buildWindowScale));
        Screen.SetResolution(width, height, FullScreenMode.Windowed);
#endif
    }
}
