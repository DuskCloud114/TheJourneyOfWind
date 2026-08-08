using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    private BoxCollider2D boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("InteractionPoint 未找到 BoxCollider2D 组件，请检查预制体设置。");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                playerInteract.IsInteracting = true;
                Debug.Log("玩家进入交互点范围，允许交互。");
            }
            else Debug.LogError("玩家对象上未找到 PlayerInteract 组件，无法设置交互状态。");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                playerInteract.IsInteracting = false;
                Debug.Log("玩家离开交互点范围，禁止交互。");
            }
            else Debug.LogError("玩家对象上未找到 PlayerInteract 组件，无法设置交互状态。");
        }
    }
}
