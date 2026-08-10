using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDash : MonoBehaviour
{
    private DashInputAction dashInputAction;
    [Header("其他组件引用")]
    private SpriteRenderer spriteRenderer;
    private PlayerAnim playerAnim;

    [Header("冲刺数据")]
    [SerializeField] private float dashSpeed = -1;
    private Vector2 dashDirection;
    private Rigidbody2D rb;

    [Header("冲刺状态")]
    [SerializeField] private bool isDashing = false;
    public bool IsDashing { get { return isDashing; } set { } }
    [SerializeField] private float dashTime = -1;
    [SerializeField] private int maxDashCount = 1;
    [SerializeField] private int dashCount = 1;
    private float dashTimer = 0;

    void Awake()
    {
        dashInputAction = new DashInputAction();
        playerAnim = this.gameObject.GetComponent<PlayerAnim>();
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        dashInputAction.Enable();

        dashInputAction.Normal.Direction.performed += GetDashDirection;
        dashInputAction.Normal.Direction.canceled += GetDashDirection;

        dashInputAction.Normal.Dash.performed += Dash;
        dashInputAction.Normal.Dash.canceled += Dash;
    }

    void OnDisable()
    {
        dashInputAction.Normal.Direction.performed -= GetDashDirection;
        dashInputAction.Normal.Direction.canceled -= GetDashDirection;

        dashInputAction.Normal.Dash.performed -= Dash;
        dashInputAction.Normal.Dash.canceled -= Dash;

        dashInputAction.Disable();
    }

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("玩家预制体缺少 Rigidbody2D 组件，请检查玩家身上是否挂载了 Rigidbody2D 组件");

        if (spriteRenderer == null) Debug.LogError("玩家预制体缺少 SpriteRenderer 组件，请检查玩家身上是否挂载了 SpriteRenderer 组件");

        if (dashSpeed < 0) Debug.LogError("玩家预制体的 dashSpeed 有误，请检查 Inspector 设置");
        if (dashTime < 0) Debug.LogError("玩家预制体的 dashTime 有误，请检查 Inspector 设置");
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimer += Time.fixedDeltaTime;
            if (dashTimer >= dashTime)
            {
                isDashing = false;
                playerAnim.SetIsDashing(isDashing);
                dashTimer = 0;
            }
        }
    }

    void GetDashDirection(InputAction.CallbackContext context)
    {
        dashDirection = context.ReadValue<Vector2>();
        if (dashDirection == Vector2.zero)
        {
            dashDirection = new Vector2(spriteRenderer.flipX ? -1 : 1, 0);
            if (dashDirection.x < 0) spriteRenderer.flipX = true;
            else if (dashDirection.x > 0) spriteRenderer.flipX = false;
        }
    }

    void Dash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("按下了冲刺");
            if (dashCount > 0)
            {
                isDashing = true;
                playerAnim.SetIsDashing(isDashing);

                rb.velocity = dashDirection.normalized * dashSpeed;

                dashTimer = 0;
                dashCount--;
            }
            else return;
        }
    }

    public void ResetDashCount()
    {
        dashCount = maxDashCount;
    }

    public void SetMaxDashCount(int count)
    {
        maxDashCount = count;
    }
}
