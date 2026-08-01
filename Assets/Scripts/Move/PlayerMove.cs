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

    [Header("水平移动数据")]
    [SerializeField] private float runSpeed = -1;
    [SerializeField] private float runUpTime = -1;
    private float runUpTimer;

    [Header("跳跃数据")]
    [SerializeField] private float jumpSpeed = -1;
    [SerializeField] private float fastJumpSpeed = -1;
    [SerializeField] private float maxJumpTime = -1;
    [SerializeField] private float jumpCount = 1;
    private float jumpTimer;

    [Header("地面检测")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float checkDistance = -1;
    [SerializeField] private float lastGroundedTime = -1;
    [SerializeField] private float coyoteTime = -1;
    [SerializeField] private bool isGrounded;
    void Awake()
    {
        moveInputAction = new MoveInputAction();
    }

    void OnEnable()
    {
        moveInputAction.Enable();
        moveInputAction.Normal.Run.performed += ChangeRunState;
        moveInputAction.Normal.Run.canceled += ChangeRunState;

        moveInputAction.Normal.Jump.performed += JumpUpdate;
        moveInputAction.Normal.Jump.canceled += JumpUpdate;
    }

    void OnDisable()
    {
        moveInputAction.Normal.Run.performed -= ChangeRunState;
        moveInputAction.Normal.Run.canceled -= ChangeRunState;

        moveInputAction.Normal.Jump.performed -= JumpUpdate;
        moveInputAction.Normal.Jump.canceled -= JumpUpdate;
        moveInputAction.Disable();
    }

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("玩家预制体缺少 Rigidbody2D，请检查玩家身上是否挂载了 Rigidbody2D 组件");

        if (runSpeed <= 0) Debug.LogError("玩家预制体的 runSpeed 速度有误，请检查 inspector");
        if (runUpTime < 0) Debug.LogError("玩家预制体的 runUpTime 时间有误，请检查 inspector");

        if (jumpSpeed <= 0) Debug.LogError("玩家预制体的 jumpSpeed 速度有误，请检查 inspector");
        if (fastJumpSpeed <= 0) Debug.LogError("玩家预制体的 fastJumpSpeed 速度有误，请检查 inspector");
        if (maxJumpTime <= 0) Debug.LogError("玩家预制体的 maxJumpTime 时间有误，请检查 inspector");
        if (jumpCount <= 0) Debug.LogError("玩家预制体的 jumpCount 次数有误，请检查 inspector");

        groundCheck = this.gameObject.transform.Find("groundCheck").gameObject.transform;
        if (groundCheck == null) Debug.LogError("玩家预制体缺少 groundCheck，请检查玩家子物体是否挂载了 groundCheck 组件");
        if (groundLayer == 0) Debug.LogError("玩家预制体的 groundLayer 层级有误，请检查 inspector");
        if (checkDistance < 0) Debug.LogError("玩家预制体的 checkDistance 距离有误，请检查 inspector");
        if (coyoteTime <= 0) Debug.LogError("玩家预制体的 coyoteTime 时间有误，请检查 inspector");

    }

    void FixedUpdate()
    {
        CheckGrounded();

        switch (runState)
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
        runUpTimer = 0;
        if (isAdd)
        {
            if (runUpTimer < runUpTime) runUpTimer += Time.fixedDeltaTime;
            else runUpTimer = runUpTime;
            rb.velocity = new Vector2(direction * runSpeed * Mathf.Clamp01(runUpTimer / runUpTime), rb.velocity.y);
        }
        else
        {
            if (runUpTimer < runUpTime) runUpTimer += Time.fixedDeltaTime;
            else runUpTimer = runUpTime;
            rb.velocity = new Vector2(rb.velocity.x * (1 - Mathf.Clamp01(runUpTimer / runUpTime)), rb.velocity.y);
        }
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);

        if (hit.collider != null)
        {
            lastGroundedTime = Time.time;
            isGrounded = true;
            if (jumpCount == 0) jumpCount = 1;
        }
        else
        {
            isGrounded = false;
        }
    }

    void JumpUpdate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (isGrounded || (Time.time - lastGroundedTime) < coyoteTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                jumpCount--;

                lastGroundedTime = -999f;
            }
        }
        if (context.canceled)
        {
            if (rb.velocity.y > fastJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, fastJumpSpeed);
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(
            groundCheck.position,
            groundCheck.position + Vector3.down * checkDistance
        );
    }
}
