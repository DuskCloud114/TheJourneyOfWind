using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum FireState
{
    burning,
    extinguished
}

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private FireState fireState = FireState.burning;
    [SerializeField] private float spawnCloudInterval = -1f;
    [SerializeField] private float spawnCloudTimer;
    [SerializeField] private float spawnOffsetDistance = -1f;

    void Start()
    {
        if (cloudPrefab == null) Debug.LogError("Fire " + gameObject.name + "上未挂载 cloud 预制体，请检查 Fire 预制体设置");

        if (spawnCloudInterval <= 0) Debug.LogError("Fire " + gameObject.name + "的 spawnCloudInterval 未设置，请检查 Fire 预制体设置");

        if (spawnOffsetDistance <= 0) Debug.LogError("Fire " + gameObject.name + "的 spawnOffsetDistance 未设置，请检查 Fire 预制体设置");

    }

    void Update()
    {
        if (fireState == FireState.burning)
        {
            spawnCloudTimer += Time.deltaTime;
            if (spawnCloudTimer >= spawnCloudInterval)
            {
                SpawnCloud();
                spawnCloudTimer = 0f;
            }
        }
    }

    public void SwitchState(WindBallState windBallState)
    {
        if (windBallState == WindBallState.strong)
        {
            fireState = FireState.extinguished;
            spawnCloudTimer = 0f;
            Death death = GetComponent<Death>();
            if (death != null) death.enabled = false;
        }
        if (windBallState == WindBallState.weak)
        {
            fireState = FireState.burning;
            Death death = GetComponent<Death>();
            if (death != null) death.enabled = true;
        }
    }

    public void SpawnCloud()
    {
        if (cloudPrefab != null)
        {
            GameObject cloud = Instantiate(cloudPrefab, transform.position + Vector3.up * spawnOffsetDistance, Quaternion.identity);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.up * spawnOffsetDistance);
    }

}
