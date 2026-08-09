using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WindBallState
{
    none,
    weak,
    strong
}

public class WindBall : MonoBehaviour
{
    [SerializeField] WindBallState windBallState = WindBallState.none;
    public WindBallState WindBallStateValue { get { return windBallState; } }
    [SerializeField] private float speed = -1f;
    [SerializeField] private float lifeTime = -1f;
    private float lifeTimer = 0f;
    private CircleCollider2D circleCollider;
    private Rigidbody2D rb;
    private Vector2 direction;
    [SerializeField] private LayerMask groundLayer;

    void Awake()
    {
        if (windBallState == WindBallState.none) Debug.LogError("风弹状态未设置，请检查预制体设置");

        if (speed <= 0) Debug.LogError("风弹速度未设置，请检查预制体设置");

        if (lifeTime <= 0) Debug.LogError("风弹存在时长未设置，请检查预制体设置");

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

        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void Init(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Fire"))
        {
            Debug.Log("风弹击中火焰");
            collider.GetComponent<Fire>()?.SwitchState(windBallState);
            Destroy(gameObject);
        }

        else if (collider.CompareTag("Cloud"))
        {
            Debug.Log("风弹击中云");
            collider.GetComponent<Cloud>()?.ChangeDirection(direction);
            Destroy(gameObject);
        }

        else if (collider.CompareTag("Barrier"))
        {
            Debug.Log("风弹击中障碍物");
            Barrier barrier = collider.GetComponent<Barrier>();
            if (barrier != null)
            {
                barrier.TakeDamage(windBallState == WindBallState.weak ? 1 : 2);
            }
            Destroy(gameObject);
        }

        else if ((1 << collider.gameObject.layer) == groundLayer.value)
        {
            Destroy(gameObject);
        }
    }
}
