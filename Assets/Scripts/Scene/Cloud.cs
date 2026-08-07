using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private float speed = -1f;
    [SerializeField] private float lifeTime = -1f;
    private float lifeTimer = 0f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    private Vector2 moveDirection = Vector2.up;

    void Start()
    {
        if (speed <= 0) Debug.LogError("云的速度未设置，请检查预制体设置");

        if (lifeTime <= 0) Debug.LogError("云的存在时长未设置，请检查预制体设置");

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("云未挂载 RigidBody2D 组件，请检查预制体设置");
        rb.gravityScale = 0f;

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null) Debug.LogError("云未挂载 BoxCollider2D 组件，请检查预制体设置");
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = moveDirection.normalized * speed;
        Debug.Log("云的移动方向为: " + moveDirection.normalized + "，速度为: " + speed);
    }

    public void ChangeDirection(Vector2 dir)
    {
        moveDirection = dir;
    }
}
