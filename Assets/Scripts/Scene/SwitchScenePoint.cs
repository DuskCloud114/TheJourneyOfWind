using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchScenePoint : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private string targetSceneName;

    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null) Debug.LogError(gameObject.name + "缺失 BoxCollider2D 组件，请检查 SwitchScenePoint 预制体设置");
        
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("SwitchScenePoint: " + gameObject.name + " 触发了切换场景点，目标场景为: " + targetSceneName);
        if (collider.CompareTag("Player"))
        {
            GameManager.Instance.SwitchScene(targetSceneName);
        }
    }
}
