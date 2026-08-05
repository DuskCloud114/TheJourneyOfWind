using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (boxCollider == null) Debug.LogError("死亡点:" + gameObject.name + "身上缺少 BoxCollider2D 组件，请检查挂载了该脚本的物体的组件设置");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("玩家死亡，回到上次存档点");

            other.transform.position = ProcessManager.Instance.LastSpawnPoint;
        }
    }
}
