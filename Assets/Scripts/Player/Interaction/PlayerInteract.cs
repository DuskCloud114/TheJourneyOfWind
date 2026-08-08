using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private bool isInteracting = false;
    public bool IsInteracting { get { return isInteracting; } set { isInteracting = value; } }
    private InteractInputAction interactInputAction;
    private InteractionPoint currentInteractionPoint;

    void Awake()
    {
        if (interactInputAction == null)
        {
            interactInputAction = new InteractInputAction();
        }
    }

    void Start()
    {

    }

    void OnEnable()
    {
        interactInputAction.Enable();
        interactInputAction.Normal.Interact.performed += Interact;
        interactInputAction.Normal.Interact.canceled += Interact;
    }

    void OnDisable()
    {
        interactInputAction.Normal.Interact.performed -= Interact;
        interactInputAction.Normal.Interact.canceled -= Interact;
        interactInputAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCurrentInteractionPoint(InteractionPoint interactionPoint)
    {
        currentInteractionPoint = interactionPoint;
        isInteracting = true;
    }

    public void RemoveCurrentInteractionPoint(InteractionPoint interactionPoint)
    {
        if (currentInteractionPoint == interactionPoint)
        {
            currentInteractionPoint = null;
            isInteracting = false;
            UIManager.Instance.Hide(); // 隐藏对话框
        }
    }

    void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("玩家按下交互键");
            if (isInteracting && currentInteractionPoint != null)
            {
                string dialogue = currentInteractionPoint.GetDialogue();
                if (!string.IsNullOrEmpty(dialogue))
                {
                    UIManager.Instance.ShowDialogue(dialogue);
                }
                else
                {
                    UIManager.Instance.ShowDialogue("当前交互点没有对话内容。");
                }
            }
            else
            {
                Debug.LogWarning("玩家不在交互范围内或没有设置当前交互点。");
            }
        }
        else if (context.canceled)
        {
            Debug.Log("玩家松开交互键");
        }
    }
}
