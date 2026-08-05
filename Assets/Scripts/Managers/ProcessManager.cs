using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessManager : MonoBehaviour
{
    public static ProcessManager Instance;

    private Vector2 lastSpawnPoint;
    public Vector2 LastSpawnPoint => lastSpawnPoint;

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
    }

    public void SetLastSpawnPosition(Vector2 position)
    {
        lastSpawnPoint = position;
    }
}
