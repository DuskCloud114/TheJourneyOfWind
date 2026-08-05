using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null) Debug.LogError("生成点:" + gameObject.name + "身上缺少 BoxCollider2D 组件，请检查 SpawnPoint 预制体设置");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ProcessManager.Instance.SetLastSpawnPosition(transform.position);
        }
    }
}
