using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindManager : MonoBehaviour
{
    public static WindManager Instance;
    private GameObject player;
    private PlayerMove playerMove;

    [SerializeField] private bool isPlayerInWind = false;
    [SerializeField] private float horVelocity = 0;
    [SerializeField] private float verVelocity = 0;
    [SerializeField] private float horImpulse = 0;
    [SerializeField] private float verImpulse = 0;

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

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError("WindManager 未找到场景中的 Player，请检查场景设置或 Player 设置");
        else playerMove = player.GetComponent<PlayerMove>();
    }

    void Update()
    {
        if (!isPlayerInWind)
        {
            horVelocity = 0;
            verVelocity = 0;
            horImpulse = 0;
            verImpulse = 0;
        }

        if (playerMove != null)
        {
            playerMove.SetVelocityAccumulation(new Vector2(horVelocity, verVelocity));
            playerMove.SetImpulseAccumulation(new Vector2(horImpulse, verImpulse));
        }
    }


    public void SetPlayerInWind(bool isInWind)
    {
        isPlayerInWind = isInWind;
    }

    public void GetWindImpulse(Vector2 windDirection, float windStrength)
    {
        Vector2 direction = windDirection.normalized;
        horImpulse += direction.x * windStrength;
        verImpulse += direction.y * windStrength;
    }

    public Vector2 GetWindImpulseAccumulation()
    {
        return new Vector2(horImpulse, verImpulse);
    }

    public void CalculateWindVelocity(Vector2 windDirection, float windStrength)
    {
        horVelocity += windDirection.x * windStrength;
        verVelocity += windDirection.y * windStrength;
    }

    public Vector2 GetVelocityAccumulation()
    {
        return new Vector2(horVelocity, verVelocity);
    }


}
