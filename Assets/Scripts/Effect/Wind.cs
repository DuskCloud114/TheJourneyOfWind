using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WindDirection
{
    up,
    down,
    left,
    right,
    upperLeft,
    upperRight,
    leftLower,
    lowerRight
}

enum WindDuration
{
    permanent,
    intermittent
}

enum WindStrength
{
    none,
    weak,
    medium,
    strong
}

enum WindType
{
    none,
    impulse,
    sustain
}

public class Wind : MonoBehaviour
{
    public Action<Wind, bool> WindStateChanged;

    bool isOpen = true;
    public bool IsOpen { get { return isOpen; } set { } }


    [Header("风相关属性枚举")]
    [SerializeField] private WindDirection windDirection = WindDirection.right;
    [SerializeField] private WindDuration windDuration = WindDuration.intermittent;
    [SerializeField] private WindStrength windStrength = WindStrength.none;
    [SerializeField] private WindType windType = WindType.none;
    [SerializeField] private BoxCollider2D affectedArea;


    [Header("风强度对应数值")]
    [SerializeField] private float weakSpeed = -1;

    [SerializeField] private float mediumSpeed = -1;

    [SerializeField] private float strongSpeed = -1;
    [SerializeField] private float verAcceleration = -1;
    [SerializeField] private float weakImpulse = -1;
    [SerializeField] private float mediumImpulse = -1;
    [SerializeField] private float strongImpulse = -1;

    [Header("风持续时间")]
    [SerializeField] private float windExistTime = -1;
    public float WindExistTime { get { return windExistTime; } set { } }

    [Header("风的开关间隔")]
    [SerializeField] private float windInterval = -1;
    private float windIntervalTimer = 0f;
    private float windExistTimer = 0f;
    public float WindInterval { get { return windInterval; } set { } }
    private bool hasApplied;
    private bool isPlayerInside = false;

    void Start()
    {
        if (windType == WindType.none) Debug.LogError(gameObject.name + "风的类型设置有误，请检查 Inspector 设置");


        if (weakSpeed <= 0 || mediumSpeed <= 0 || strongSpeed <= 0) Debug.LogError(gameObject.name + "风的强度数值设置有误，请检查预制体和 inspector 设置");
        if (weakSpeed >= mediumSpeed || weakSpeed >= strongSpeed || mediumSpeed >= strongSpeed) Debug.LogError(gameObject.name + "风的速度数值梯度设置有误，请按照 weakSpeed < mediumSpeed < strongSpeed 检查预制体和 inspector 设置");

        if (weakImpulse <= 0 || mediumImpulse <= 0 || strongImpulse <= 0) Debug.LogError(gameObject.name + "风的冲击力数值设置有误，请检查预制体和 inspector 设置");
        if (weakImpulse >= mediumImpulse || weakImpulse >= strongImpulse || mediumImpulse >= strongImpulse) Debug.LogError(gameObject.name + "风的冲击力数值梯度设置有误，请按照 weakImpulse < mediumImpulse < strongImpulse 检查预制体和 inspector 设置");
        
        if (verAcceleration <= 0) Debug.LogError(gameObject.name + "风的垂直加速度数值设置有误，请检查预制体和 inspector 设置");

        if (windDuration == WindDuration.intermittent)
        {
            if (windExistTime <= 0 || windInterval <= 0) Debug.LogError(gameObject.name + "风的持续时间或间隔时间设置有误，请检查预制体和 inspector 设置");
        }

        affectedArea = this.gameObject.GetComponent<BoxCollider2D>();
        if (affectedArea == null) Debug.LogError(gameObject.name + "风预制体缺少 BoxCollider2D 组件，请检查风预制体身上是否挂载了 BoxCollider2D 组件");
    }

    void Update()
    {
        if (windDuration == WindDuration.intermittent) IntermittentWindControl();
    }

    public Vector2 GetWindDirection()
    {
        switch (windDirection)
        {
            case WindDirection.up:
                return Vector2.up;
            case WindDirection.down:
                return Vector2.down;
            case WindDirection.left:
                return Vector2.left;
            case WindDirection.right:
                return Vector2.right;
            case WindDirection.upperLeft:
                return new Vector2(-1, 1).normalized;
            case WindDirection.upperRight:
                return new Vector2(1, 1).normalized;
            case WindDirection.leftLower:
                return new Vector2(-1, -1).normalized;
            case WindDirection.lowerRight:
                return new Vector2(1, -1).normalized;
            default:
                return Vector2.zero;
        }
    }

    public float GetWindSpeed()
    {
        switch (windStrength)
        {
            case WindStrength.none:
                return 0f;
            case WindStrength.weak:
                return weakSpeed;
            case WindStrength.medium:
                return mediumSpeed;
            case WindStrength.strong:
                return strongSpeed;
            default:
                return 0f;
        }
    }

    public float GetWindImpulse()
    {
        switch (windStrength)
        {
            case WindStrength.none:
                return 0f;
            case WindStrength.weak:
                return weakImpulse;
            case WindStrength.medium:
                return mediumImpulse;
            case WindStrength.strong:
                return strongImpulse;
            default:
                return 0f;
        }
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = true;

            if (windType == WindType.impulse)
            {
                ApplyImpulse();
            }
            else
            {
                ApplyVelocity();
            }

        }

        else if (collision.CompareTag("Cloud"))
        {
            Cloud cloud = collision.GetComponent<Cloud>();
            if (cloud != null)
            {
                cloud.EnterWind(this);
            }
        }
    }

    void ApplyImpulse()
    {
        WindManager.Instance.GetWindImpulse(GetWindDirection(), GetWindImpulse());
        // Debug.Log("玩家进入风区 " + gameObject.name + "，风的方向为：" + GetWindDirection() + "，风的冲击力为：" + GetWindImpulse() + "，目前累积冲量为：" + WindManager.Instance.GetWindImpulseAccumulation());
    }

    void ApplyVelocity()
    {
        if (hasApplied) return;

        hasApplied = true;
        Vector2 direction = GetWindDirection();
        float speed = GetWindSpeed();
        if (direction.y != 0)
        {
            speed *= GetWindSpeed();
            speed += verAcceleration;
        }
        WindManager.Instance.CalculateWindVelocity(direction, speed);
        // Debug.Log("玩家进入风区 " + gameObject.name + "，风的方向为：" + GetWindDirection() + "，风的速度为：" + GetWindSpeed() + "，目前累积速度为：" + WindManager.Instance.GetWindVelocityAccumulation());
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInside = false;

            if (windType == WindType.impulse)
            {
                RemoveImpulse();
            }
            else
            {
                RemoveVelocity();
            }
        }
        else if (collision.CompareTag("Cloud"))
        {
            Cloud cloud = collision.GetComponent<Cloud>();
            if (cloud != null)
            {
                cloud.ExitWind(this);
            }
        }
    }

    void RemoveImpulse()
    {
        WindManager.Instance.GetWindImpulse(GetWindDirection(), -GetWindImpulse());
        // Debug.Log("玩家离开风区 " + gameObject.name + "，风的方向为：" + GetWindDirection() + "，风的冲击力为：" + GetWindImpulse() + "，目前累积冲量为：" + WindManager.Instance.GetWindImpulseAccumulation());
    }

    void RemoveVelocity()
    {
        if (!hasApplied) return;

        hasApplied = false;
        Vector2 direction = GetWindDirection();
        float speed = GetWindSpeed();
        if (direction.y != 0)
        {
            speed *= GetWindSpeed();
            speed += verAcceleration;
        }
        WindManager.Instance.CalculateWindVelocity(direction, -speed);
        // Debug.Log("玩家离开风区 " + gameObject.name + "，风的方向为：" + GetWindDirection() + "，风的速度为：" + GetWindSpeed() + "，目前累积速度为：" + WindManager.Instance.GetWindVelocityAccumulation());
    }

    private void IntermittentWindControl()
    {
        if (windDuration != WindDuration.intermittent) return;
        if (isOpen)
        {
            windExistTimer += Time.deltaTime;
            if (windExistTimer >= windExistTime)
            {
                SwitchWindState(false);
                windExistTimer = 0f;
                // Debug.Log("风区 " + gameObject.name + " 风关闭");
            }
        }
        else
        {
            windIntervalTimer += Time.deltaTime;
            if (windIntervalTimer >= windInterval)
            {
                SwitchWindState(true);
                windIntervalTimer = 0f;
                // Debug.Log("风区 " + gameObject.name + " 风开启");
            }
        }
    }

    public void SwitchWindState(bool isOpen)
    {
        if (this.isOpen == isOpen) return;

        this.isOpen = isOpen;
        WindStateChanged?.Invoke(this, isOpen);
        if (isOpen && isPlayerInside)
        {
            ApplyVelocity();
        }
        else
        {
            RemoveVelocity();
        }

    }


}
