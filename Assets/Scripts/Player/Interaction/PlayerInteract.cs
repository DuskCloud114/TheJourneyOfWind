using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private bool isInteracting = false;
    public bool IsInteracting { get { return isInteracting; } set { isInteracting = value; } }
    private InteractInputAction interactInputAction;

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

    void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("玩家按下交互键");
        }
        else if (context.canceled)
        {
            Debug.Log("玩家松开交互键");
        }
    }
}
