using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum RunState
{
    left,
    stay,
    right
}

public class PlayerMove : MonoBehaviour
{
    MoveInputAction moveInputAction;
    [SerializeField] private RunState runState = RunState.stay;
    public Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float runUpTime;
    private float timer;

    void Awake()
    {
        moveInputAction = new MoveInputAction();
    }

    void OnEnable()
    {
        moveInputAction.Enable();
        moveInputAction.Normal.Run.performed += ChangeRunState;
        moveInputAction.Normal.Run.canceled += ChangeRunState;
    }

    void OnDisable()
    {
        moveInputAction.Disable();
        moveInputAction.Normal.Run.performed -= ChangeRunState;
        moveInputAction.Normal.Run.canceled -= ChangeRunState;
    }

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("玩家配置缺少 Rigidbody2D，请检查玩家身上是否挂载了 Rigidbody2D 组件");
    }

    void FixedUpdate()
    {
        switch(runState)
        {
            case RunState.left:
                SpeedUpdate(-1, true);
                break;
            case RunState.right:
                SpeedUpdate(1, true);
                break;
            case RunState.stay:
                SpeedUpdate(0, false);
                break;
        }
    }

    void ChangeRunState(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() < 0)
        {
            if (runState != RunState.right) runState = RunState.left;
        }
        else if (context.ReadValue<float>() > 0)
        {
            if (runState != RunState.left) runState = RunState.right;
        }
        else
        {
            runState = RunState.stay;
        }
    }

    void SpeedUpdate(int direction, bool isAdd)
    {
        timer = 0;
        if (isAdd)
        {
            if (timer < runUpTime) timer += Time.fixedDeltaTime;
            else timer = runUpTime;
            rb.velocity = new Vector2(direction * speed * Mathf.Clamp01(timer / runUpTime), rb.velocity.y);
        }
        else
        {
            if (timer < runUpTime) timer += Time.fixedDeltaTime;
            else timer = runUpTime;
            rb.velocity = new Vector2(rb.velocity.x * (1 - Mathf.Clamp01(timer / runUpTime)), rb.velocity.y);
        }
    }
}
