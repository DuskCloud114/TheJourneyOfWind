using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    [SerializeField] private string interactionPrompt = "按 F 检索"; // 提示信息 
    [SerializeField] private DialogueData dialogueData; // 对话数据
    private int index = 0; // 当前对话索引

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("InteractionPoint 未找到 BoxCollider2D 组件，请检查预制体设置。");
        }
    }

    public string GetDialogue()
    {
        if (dialogueData == null) 
        {
            Debug.LogError("DialogueData 未设置，请在 Inspector 中分配对话数据。");
            return null;
        }
        else
        {
            string dialogue = dialogueData.dialogueLines[index];
            index = Mathf.Min(index + 1, dialogueData.dialogueLines.Length - 1); // 确保索引不超过数组长度
            return dialogue;
        }
        
    }

    public int GetDialogueLength()
    {
        if (dialogueData == null) 
        {
            Debug.LogError("DialogueData 未设置，请在 Inspector 中分配对话数据。");
            return 0;
        }
        else
        {
            return dialogueData.dialogueLines.Length;
        }
    }

    public int GetDialogueIndex()
    {
        return index;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInteract playerInteract = other.GetComponent<PlayerInteract>();
            if (playerInteract != null)
            {
                playerInteract.IsInteracting = true;
                playerInteract.SetCurrentInteractionPoint(this);
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
                playerInteract.RemoveCurrentInteractionPoint(this);
                Debug.Log("玩家离开交互点范围，禁止交互。");
            }
            else Debug.LogError("玩家对象上未找到 PlayerInteract 组件，无法设置交互状态。");
        }
    }
}
