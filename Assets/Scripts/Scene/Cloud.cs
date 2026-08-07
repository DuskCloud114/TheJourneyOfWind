using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private Dictionary<Wind, Vector2> windContributions = new Dictionary<Wind, Vector2>();

    [Header("其他组件引用")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider;
    private Vector2 moveDirection = Vector2.up;

    [Header("基本数据配置")]
    [SerializeField] private float speed = -1f;
    [SerializeField] private float lifeTime = -1f;
    private float lifeTimer = 0f;
    private Vector2 velocityAccumulation;

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
        rb.velocity = moveDirection.normalized * speed + velocityAccumulation;
        Debug.Log("云的移动方向为: " + moveDirection.normalized + "，速度为: " + moveDirection.normalized * speed + velocityAccumulation);
    }

    void OnDestroy()
    {
        foreach (Wind wind in windContributions.Keys)
        {
            if (wind != null) wind.WindStateChanged -= OnWindStateChanged;
        }

        windContributions.Clear();
    }

    public void ChangeDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    public void EnterWind(Wind wind)
    {
        if (wind == null) return;

        if (windContributions.ContainsKey(wind)) return;
        
        windContributions.Add(wind, Vector2.zero);
        wind.WindStateChanged += OnWindStateChanged;
        if (wind.IsOpen) AddVelocity(wind);
    }

    public void ExitWind(Wind wind)
    {
        if (wind == null) return;

        if (!windContributions.ContainsKey(wind)) return;
        
        wind.WindStateChanged -= OnWindStateChanged;
        RemoveVelocity(wind);
        windContributions.Remove(wind);

    }

    public void OnWindStateChanged(Wind wind, bool isOpen)
    {
        if (wind == null) return;
        if (!windContributions.ContainsKey(wind)) return;

        if (isOpen) AddVelocity(wind);
        else RemoveVelocity(wind);
    }

    void AddVelocity(Wind wind)
    {
        if (wind == null) return;
        if (windContributions.ContainsKey(wind) && windContributions[wind] == Vector2.zero)
        {
            Vector2 windVelocity = wind.GetWindDirection() * wind.GetWindSpeed();
            windContributions[wind] = windVelocity;

            velocityAccumulation += windVelocity;
        }
    }

    void RemoveVelocity(Wind wind)
    {
        if (wind == null) return;
        if (windContributions.ContainsKey(wind) && windContributions[wind] != Vector2.zero)
        {
            velocityAccumulation -= windContributions[wind];
            windContributions[wind] = Vector2.zero;            
        }
    }
}
