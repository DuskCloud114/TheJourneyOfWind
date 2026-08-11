using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerPrefab;
    public GameObject UIManager;
    public GameObject audioManager;
    public GameObject sceneManager;
    public GameObject skillsManager;
    public GameObject processManager;
    public GameObject cameraManager;
    public GameObject windManager;
    public Vector2 lastSpawnPoint;

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

        if (playerPrefab == null) Debug.LogError("GameManager上未挂载 playerPrefab，请检查 GameManager 预制体设置");
        if (UIManager == null) Debug.LogError("GameManager上未挂载 UIManager，请检查 GameManager 预制体设置");
        // if (audioManager == null) Debug.LogError("GameManager上未挂载 audioManager，请检查 GameManager 预制体设置");
        // if (sceneManager == null) Debug.LogError("GameManager上未挂载 sceneManager，请检查 GameManager 预制体设置");
        if (skillsManager == null) Debug.LogError("GameManager上未挂载 skillsManager，请检查 GameManager 预制体设置");
        if (processManager == null) Debug.LogError("GameManager上未挂载 processManager，请检查 GameManager 预制体设置");
        if (cameraManager == null) Debug.LogError("GameManager上未挂载 cameraManager，请检查 GameManager 预制体设置");
        if (windManager == null) Debug.LogError("GameManager上未挂载 windManager，请检查 GameManager 预制体设置");

        lastSpawnPoint = GameObject.FindWithTag("FirstSpawnPoint")?.transform.position ?? Vector2.zero;
        InitializeManagers();
    }

    void InitializeManagers()
    {
        if (playerPrefab != null)
        {
            GameObject go = Instantiate(playerPrefab);
            if (lastSpawnPoint != Vector2.zero) go.transform.position = lastSpawnPoint;   
        }
        
        if (UIManager != null) Instantiate(UIManager);
        
        if (audioManager != null) Instantiate(audioManager);
        
        if (sceneManager != null) Instantiate(sceneManager);
        
        if (skillsManager != null) Instantiate(skillsManager);

        if (processManager != null) Instantiate(processManager);
        processManager.GetComponent<ProcessManager>().SetLastSpawnPosition(lastSpawnPoint);

        if (cameraManager != null) Instantiate(cameraManager);
        
        if (windManager != null) Instantiate(windManager);
    }
}
