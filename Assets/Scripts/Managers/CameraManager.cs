using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public Camera mainCamera;

    [SerializeField] List<RoomTransformer> rooms = new List<RoomTransformer>();
    [SerializeField] private RoomTransformer currentRoom;

    [Header("摄像机跟随设置")]
    [SerializeField] private Vector2 maxMoveBounds = new Vector2(-1f, -1f);
    [SerializeField] private float followSpeed = -1f;
    [SerializeField] private float upBound;
    [SerializeField] private float downBound;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    private bool canFollowPlayer;
    private GameObject player;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        if (mainCamera == null) Debug.LogError("场景中不存在 Camera，请检查场景中是否有 Camera 组件");

        currentRoom = GameObject.FindGameObjectWithTag("FirstRoom")?.GetComponent<RoomTransformer>();
        if (currentRoom == null) Debug.LogError("CameraManager 中未设置首个房间，请检查场景房间以及房间标签设置");

        player = GameManager.Instance.player;

        if (maxMoveBounds.x <= 0f || maxMoveBounds.y <= 0f) Debug.LogError("CameraManager 中的 maxMoveBounds 必须大于 0，请检查 CameraManager 预制体设置或 inspector 设置");

        if (followSpeed <= 0f) Debug.LogError("CameraManager 中的 followSpeed 必须大于 0，请检查 CameraManager 预制体设置或 inspector 设置");
    }

    void LateUpdate()
    {
        if (canFollowPlayer && player != null)
        {
            Vector2 playerPosition = player.transform.position;
            Vector2 targetPosition = new Vector2(0f, 0f);

            if (playerPosition.x - leftBound >= maxMoveBounds.x && rightBound - playerPosition.x >= maxMoveBounds.x)
            {
                targetPosition.x = playerPosition.x;
            }
            else if (playerPosition.x - leftBound < maxMoveBounds.x)
            {
                targetPosition.x = leftBound + maxMoveBounds.x;
            }
            else if (rightBound - playerPosition.x < maxMoveBounds.x)
            {
                targetPosition.x = rightBound - maxMoveBounds.x;
            }

            if (upBound - playerPosition.y >= maxMoveBounds.y && playerPosition.y - downBound >= maxMoveBounds.y)
            {
                targetPosition.y = playerPosition.y;
            }
            else if (upBound - playerPosition.y < maxMoveBounds.y)
            {
                targetPosition.y = upBound - maxMoveBounds.y;
            }
            else if (playerPosition.y - downBound < maxMoveBounds.y)
            {
                targetPosition.y = downBound + maxMoveBounds.y;
            }

            mainCamera.transform.position = new Vector3(
                Mathf.Lerp(mainCamera.transform.position.x, targetPosition.x, Time.deltaTime * followSpeed),
                targetPosition.y,
                mainCamera.transform.position.z
            );
        }
    }

    public void ResetState()
    {
        mainCamera = Camera.main;
        player = GameManager.Instance.player;
        currentRoom = GameObject.FindGameObjectWithTag("FirstRoom").GetComponent<RoomTransformer>();
        if (currentRoom == null) Debug.LogError("CameraManager 中未设置首个房间，请检查场景房间以及房间标签设置");
        canFollowPlayer = currentRoom.CanFollowPlayer;
    }

    public void EnterRoom(RoomTransformer room)
    {
        if (rooms.Contains(room)) return;
        rooms.Add(room);
    }

    public void ExitRoom(RoomTransformer room)
    {
        if (!rooms.Contains(room)) return;
        rooms.Remove(room);
    }

    public bool IsCurrentRoomInList(RoomTransformer room)
    {
        if (currentRoom == null || !rooms.Contains(currentRoom))
        {
            currentRoom = room;
            return false;
        }
        else return true;
    }

    public void SwitchToRoom()
    {
        if (rooms.Count == 0) return;
        currentRoom = rooms[rooms.Count - 1];
        mainCamera.transform.position = new Vector3(currentRoom.RoomCenter.x, currentRoom.RoomCenter.y, mainCamera.transform.position.z);

        canFollowPlayer = currentRoom.CanFollowPlayer;
        upBound = currentRoom.GetComponent<BoxCollider2D>().bounds.max.y;
        downBound = currentRoom.GetComponent<BoxCollider2D>().bounds.min.y;
        leftBound = currentRoom.GetComponent<BoxCollider2D>().bounds.min.x;
        rightBound = currentRoom.GetComponent<BoxCollider2D>().bounds.max.x;
    }
}
