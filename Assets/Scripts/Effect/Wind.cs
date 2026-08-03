using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WindDirection
{
    up,
    down,
    left,
    right
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

public class Wind : MonoBehaviour
{
    [Header("风相关属性枚举")]
    [SerializeField] private WindDirection windDirection = WindDirection.right;
    [SerializeField] private WindDuration windDuration = WindDuration.intermittent;
    [SerializeField] private WindStrength windStrength = WindStrength.none;

    [Header("风强度对应数值")]
    [SerializeField] private float weakSpeed = -1;

    [SerializeField] private float mediumSpeed = -1;
    
    [SerializeField] private float strongSpeed = -1;

    [Header("风持续时间")]
    [SerializeField] private float windExistTime = -1;
    public float WindExistTime { get { return windExistTime; } set { } }

    [Header("风的开关间隔")]
    [SerializeField] private float windInterval = -1;
    public float WindInterval { get { return windInterval; } set { } }
 
    void Start()
    {
        if (weakSpeed <= 0 || mediumSpeed <= 0 || strongSpeed <= 0) Debug.LogError("风的强度数值设置有误，请检查 Inspector 设置");
        if (weakSpeed >= mediumSpeed || weakSpeed >= strongSpeed || mediumSpeed >= strongSpeed) Debug.LogError("风的强度数值梯度设置有误，请按照 weakSpeed < mediumSpeed < strongSpeed 检查 Inspector 设置");

        if (windDuration == WindDuration.intermittent)
        {
            if (windExistTime <= 0 || windInterval <= 0) Debug.LogError("风的持续时间或间隔时间设置有误，请检查 Inspector 设置");
        }
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
}
