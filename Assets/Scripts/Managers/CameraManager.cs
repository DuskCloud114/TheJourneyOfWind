using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;
    public Camera mainCamera;

    [SerializeField] List<RoomTransformer> rooms = new List<RoomTransformer>();
    [SerializeField] private RoomTransformer currentRoom;
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

        if (currentRoom == null) Debug.LogError("CameraManager 中未设置当前房间，请检查 CameraManager 组件设置");

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) Debug.LogError("场景中不存在 Player，请检查场景中是否有 Player 组件");
    }

    void LateUpdate()
    {
        if (canFollowPlayer && player != null)
        {
            Vector3 targetPosition = new Vector3(player.transform.position.x, player.transform.position.y, mainCamera.transform.position.z);
            mainCamera.transform.position = targetPosition;
        }
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

    public void SwitchToRoom()
    {
        if (rooms.Count == 0) return;
        currentRoom = rooms[rooms.Count - 1];
        mainCamera.transform.position = new Vector3(currentRoom.RoomCenter.x, currentRoom.RoomCenter.y, mainCamera.transform.position.z);
        canFollowPlayer = currentRoom.CanFollowPlayer;
    }
}
