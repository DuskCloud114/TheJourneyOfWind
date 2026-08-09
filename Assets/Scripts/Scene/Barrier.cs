using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Barrier : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private int barrierHealth = 3; // 障碍物的生命值
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null) Debug.LogError("障碍物缺少 BoxCollider2D 组件，请检查 Barrier 预制体设置");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        barrierHealth -= damage;
        if (barrierHealth <= 0)
        {
            Destroy(gameObject); // 当生命值为0时销毁障碍物
        }
    }
}
