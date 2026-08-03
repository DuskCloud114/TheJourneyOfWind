using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager Instance;

    [SerializeField] private bool isPlayerInWind = false;
    [SerializeField] private float horVelocity = 0;
    [SerializeField] private float verVelocity = 0;

    // TODO: 通过事件的方式通知场景物体动画改变

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void SetPlayerInWind(bool isInWind)
    {
        isPlayerInWind = isInWind;
    }

    public void calculateWindVelocity(Vector2 windDirection, float windStrength)
    {
        horVelocity += windDirection.x * windStrength;
        verVelocity += windDirection.y * windStrength;
    }

    public Vector2 GetVelocityAccumulation()
    {
        return new Vector2(horVelocity, verVelocity);
    }


}
