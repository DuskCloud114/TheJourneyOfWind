using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;

    void Start()
    {
        if (cloudPrefab == null) Debug.LogError("Fire " + gameObject.name + "上未挂载 cloud 预制体，请检查 Fire 预制体设置");

    }

    public void SpawnCloud()
    {
        if (cloudPrefab != null)
        {
            GameObject cloud = Instantiate(cloudPrefab, transform.position, Quaternion.identity);
        }
    }

}
