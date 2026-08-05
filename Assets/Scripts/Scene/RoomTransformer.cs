using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransformer : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private Vector2 roomCenter;
    public Vector2 RoomCenter => roomCenter;

    [SerializeField] private bool canFollowPlayer = false;
    public bool CanFollowPlayer => canFollowPlayer;
    
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null) Debug.LogError("Room 预制体缺少 BoxCollider2D 组件，请检查 Room 预制体设置");
        roomCenter = boxCollider.bounds.center;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.Instance.EnterRoom(this);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraManager.Instance.ExitRoom(this);
            CameraManager.Instance.SwitchToRoom();
        }
    }
}
