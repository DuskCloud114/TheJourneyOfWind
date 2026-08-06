using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindBall : MonoBehaviour
{
    [SerializeField] private float speed = -1f;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        if (speed <= 0) Debug.LogError("风弹速度未设置，请检查预制体设置");
        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null) Debug.LogError("风弹未挂载 CircleCollider2D 组件，请检查预制体设置");

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("风弹未挂载 RigidBody2D 组件，请检查预制体设置");
        rb.gravityScale = 0f;
    }

    void Start()
    {
        if (speed <= 0) Debug.LogError("风弹速度未设置，请检查预制体设置");

        circleCollider = GetComponent<CircleCollider2D>();
        if (circleCollider == null) Debug.LogError("风弹未挂载 CircleCollider2D 组件，请检查预制体设置");

        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("风弹未挂载 RigidBody2D 组件，请检查预制体设置");
        rb.gravityScale = 0f;

        if (groundLayer == 0) Debug.LogError("风弹的 groundLayer 未设置，请检查预制体设置");
    }

    void Update()
    {
        if (rb != null) rb.velocity = direction * speed;
    }

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((1 << collider.gameObject.layer) == groundLayer.value)
        {
            Destroy(gameObject);
        }
    }
}
