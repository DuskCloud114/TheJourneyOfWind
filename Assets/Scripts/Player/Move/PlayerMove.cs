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

// TODO: 解决玩家和场景的摩擦问题

public class PlayerMove : MonoBehaviour
{
    MoveInputAction moveInputAction;
    [SerializeField] private RunState runState = RunState.stay;

    [Header("其他组件引用")]
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float maxGravityScale;
    private PlayerDash playerDash;
    private PlayerAnim playerAnim;

    [Header("水平移动数据")]
    [SerializeField] private float runSpeed = -1;
    [SerializeField] private float runUpTime = -1;

    [Header("跳跃数据")]
    [SerializeField] private float jumpSpeed = -1;
    [SerializeField] private float fastJumpSpeed = -1;
    [SerializeField] private float maxJumpTime = -1;
    [SerializeField] private float jumpCount = 1;
    [SerializeField] private float maxVerSpeed = -1;

    [Header("地面检测")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(0.55f, 0.1f);
    [SerializeField] private float checkDistance = -1;
    [SerializeField] private float lastGroundedTime = -1;
    [SerializeField] private float coyoteTime = -1;
    [SerializeField] private bool isGrounded;
    public bool IsGrounded { get { return isGrounded; } set { } }

    [Header("风场作用")]
    [SerializeField] private Vector2 velocityAccumulation = Vector2.zero;
    [SerializeField] private Vector2 appliedVelocity = Vector2.zero;
    [SerializeField] private Vector2 impulseAccumulation = Vector2.zero;

    void Awake()
    {
        moveInputAction = new MoveInputAction();
        playerDash = this.gameObject.GetComponent<PlayerDash>();
        playerAnim = this.gameObject.GetComponent<PlayerAnim>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
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
        if (rb == null) Debug.LogError("玩家预制体缺少 Rigidbody2D 组件，请检查玩家身上是否挂载了 Rigidbody2D 组件");
        if (rb != null) maxGravityScale = rb.gravityScale;

        if (playerDash == null) Debug.LogError("玩家预制体缺少 PlayerDash 组件，请检查玩家身上是否挂载了 PlayerDash 组件");

        if (playerAnim == null) Debug.LogError("玩家预制体缺少 PlayerAnim 组件，请检查玩家身上是否挂载了 PlayerAnim 组件");

        if (spriteRenderer == null) Debug.LogError("玩家预制体缺少 SpriteRenderer 组件，请检查玩家身上是否挂载了 SpriteRenderer 组件");

        if (runSpeed <= 0) Debug.LogError("玩家预制体的 runSpeed 速度有误，请检查 inspector");
        if (runUpTime < 0) Debug.LogError("玩家预制体的 runUpTime 时间有误，请检查 inspector");

        if (jumpSpeed <= 0) Debug.LogError("玩家预制体的 jumpSpeed 速度有误，请检查 inspector");
        if (fastJumpSpeed <= 0) Debug.LogError("玩家预制体的 fastJumpSpeed 速度有误，请检查 inspector");
        if (maxJumpTime <= 0) Debug.LogError("玩家预制体的 maxJumpTime 时间有误，请检查 inspector");
        if (jumpCount <= 0) Debug.LogError("玩家预制体的 jumpCount 次数有误，请检查 inspector");
        if (maxVerSpeed <= 0) Debug.LogError("玩家预制体的 maxVerSpeed 速度有误，请检查 inspector");

        groundCheck = this.gameObject.transform.Find("groundCheck").gameObject.transform;
        if (groundCheck == null) Debug.LogError("玩家预制体缺少 groundCheck，请检查玩家子物体是否挂载了 groundCheck 组件");
        if (groundLayer == 0) Debug.LogError("玩家预制体的 groundLayer 层级有误，请检查 inspector");
        if (groundCheckSize.x <= 0 || groundCheckSize.y <= 0) Debug.LogError("玩家预制体的 groundCheckSize 有误，请检查 inspector");
        if (checkDistance < 0) Debug.LogError("玩家预制体的 checkDistance 距离有误，请检查 inspector");
        if (coyoteTime <= 0) Debug.LogError("玩家预制体的 coyoteTime 时间有误，请检查 inspector");

    }

    void FixedUpdate()
    {
        CheckGrounded();

        if (playerDash != null && playerDash.IsDashing)
        {
            rb.gravityScale = 0;
            return;
        }
        else rb.gravityScale = maxGravityScale;

        if (playerDash != null && isGrounded) playerDash.ResetDashCount();

        rb.velocity -= new Vector2(appliedVelocity.x, 0f);
        appliedVelocity = Vector2.zero;

        switch (runState)
        {
            case RunState.left:
                SpeedUpdate(-1);
                break;
            case RunState.right:
                SpeedUpdate(1);
                break;
            case RunState.stay:
                SpeedUpdate(0);
                break;
        }

        rb.velocity += new Vector2(velocityAccumulation.x, 0);
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Min(rb.velocity.y + velocityAccumulation.y * Time.fixedDeltaTime, maxVerSpeed));
        appliedVelocity = velocityAccumulation;

        rb.AddForce(impulseAccumulation, ForceMode2D.Impulse);
    }

    public void ResetState()
    {
        runState = RunState.stay;
        velocityAccumulation = Vector2.zero;
        impulseAccumulation = Vector2.zero;
    }

    public void SetVelocityAccumulation(Vector2 velocity)
    {
        velocityAccumulation = velocity;
    }
    public void SetImpulseAccumulation(Vector2 impulse)
    {
        impulseAccumulation = impulse;
    }

    void ChangeRunState(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() < 0)
        {
            if (runState != RunState.right)
            {
                runState = RunState.left;
                spriteRenderer.flipX = true;
            }
        }
        else if (context.ReadValue<float>() > 0)
        {
            if (runState != RunState.left)
            {
                runState = RunState.right;
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            runState = RunState.stay;
        }

        playerAnim.SetIsRunning(runState != RunState.stay);
    }

    void SpeedUpdate(int direction)
    {
        float targetSpeed = direction * runSpeed;

        if (runUpTime <= 0)
        {
            rb.velocity = new Vector2(targetSpeed, rb.velocity.y);
            return;
        }

        float acceleration = runSpeed / runUpTime;
        float newSpeed = Mathf.MoveTowards(rb.velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime);
        rb.velocity = new Vector2(newSpeed, rb.velocity.y);
    }

    void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            groundCheck.position,
            groundCheckSize,
            0f,
            Vector2.down,
            checkDistance,
            groundLayer
        );

        isGrounded = hit.collider != null;
        if (isGrounded)
        {
            lastGroundedTime = Time.time;
            if (jumpCount == 0) jumpCount = 1;
        }

        playerAnim.SetIsGrounded(isGrounded);
    }

    void JumpUpdate(InputAction.CallbackContext context)
    {
        if (playerDash != null && playerDash.IsDashing) return;

        float windVelocityY = appliedVelocity.y;
        float playerVerSpeed = rb.velocity.y - windVelocityY;

        if (context.performed)
        {
            if (isGrounded || (Time.time - lastGroundedTime) < coyoteTime)
            {
                lastGroundedTime = -999f;

                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed + windVelocityY);
                jumpCount--;
                playerAnim.SetIsJumping(true);
            }
        }
        if (context.canceled)
        {
            if (playerVerSpeed > fastJumpSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, fastJumpSpeed + windVelocityY);
            }
        }

        if ((!context.performed && !context.canceled) || rb.velocity.y < 0 || isGrounded)
        {
            playerAnim.SetIsJumping(false);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Vector3 castCenter = groundCheck.position + Vector3.down * (checkDistance * 0.5f);
        Vector3 castSize = new Vector3(groundCheckSize.x, groundCheckSize.y + checkDistance, 0f);
        Gizmos.DrawWireCube(castCenter, castSize);
    }
}
