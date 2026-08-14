using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject playerPrefab;
    public GameObject player;
    public GameObject UIManager;
    public GameObject audioManager;
    public GameObject skillsManager;
    public GameObject processManager;
    public GameObject cameraManager;
    public GameObject windManager;
    public Vector2 lastSpawnPoint;

    public bool isSwitchingScene = false;

    void Awake()
    {
        if (playerPrefab == null) Debug.LogError("GameManager上未挂载 playerPrefab，请检查 GameManager 预制体设置");
        if (UIManager == null) Debug.LogError("GameManager上未挂载 UIManager，请检查 GameManager 预制体设置");
        if (audioManager == null) Debug.LogError("GameManager上未挂载 audioManager，请检查 GameManager 预制体设置");
        if (skillsManager == null) Debug.LogError("GameManager上未挂载 skillsManager，请检查 GameManager 预制体设置");
        if (processManager == null) Debug.LogError("GameManager上未挂载 processManager，请检查 GameManager 预制体设置");
        if (cameraManager == null) Debug.LogError("GameManager上未挂载 cameraManager，请检查 GameManager 预制体设置");
        if (windManager == null) Debug.LogError("GameManager上未挂载 windManager，请检查 GameManager 预制体设置");

        lastSpawnPoint = GameObject.FindWithTag("FirstSpawnPoint")?.transform.position ?? Vector2.zero;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
        }
        else
        {
            Destroy(gameObject);
        }
 
    }

    void InitializeManagers()
    {
        if (playerPrefab != null)
        {
            player = Instantiate(playerPrefab);
            if (lastSpawnPoint != Vector2.zero) player.transform.position = lastSpawnPoint;
            DontDestroyOnLoad(player);
        }

        if (UIManager != null) Instantiate(UIManager);

        if (audioManager != null) Instantiate(audioManager);

        if (skillsManager != null) Instantiate(skillsManager);

        if (processManager != null) Instantiate(processManager);
        processManager.GetComponent<ProcessManager>().SetLastSpawnPosition(lastSpawnPoint);

        if (cameraManager != null) Instantiate(cameraManager);

        if (windManager != null) Instantiate(windManager);
    }

    public void SwitchScene(string targetSceneName)
    {
        StartCoroutine(SwitchSceneCoroutine(targetSceneName));
    }

    IEnumerator SwitchSceneCoroutine(string targetSceneName)
    {
        isSwitchingScene = true;

        // 在这里可以添加切换场景的逻辑，例如淡入淡出效果等
        var async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(targetSceneName);

        while (!async.isDone)
        {
            yield return null;
        }

        lastSpawnPoint = GameObject.FindWithTag("FirstSpawnPoint")?.transform.position ?? Vector2.zero;
        player.transform.position = lastSpawnPoint;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<PlayerMove>().ResetState();

        CameraManager.Instance.ResetState();


        isSwitchingScene = false;
    }
}
