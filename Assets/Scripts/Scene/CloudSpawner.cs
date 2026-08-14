using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Vector2 initDirection = new Vector2(-1, 0);
    [SerializeField] private float spawnInterval = -1f;

    void Awake()
    {
        if (cloudPrefab == null) Debug.LogError(gameObject.name + " 未找到 cloudPrefab，请检查预制体设置");

        if (spawnInterval <= 0) Debug.LogError(gameObject.name + " 云的生成间隔未设置，请检查预制体设置");
    }

    void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    IEnumerator SpawnClouds()
    {
        while (true)
        {
            GameObject cloud = Instantiate(cloudPrefab, transform.position, Quaternion.identity);
            cloud.GetComponent<Cloud>().ChangeDirection(initDirection);
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
