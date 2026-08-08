using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SkillUseType
{
    weak,
    strong
}

public class Skill : MonoBehaviour
{
    public SkillId id;
    protected SkillInputAction skillInputAction;
    [SerializeField] protected SkillUseType skillUseType;
    [SerializeField] protected float skillInterval = -1f;
    [SerializeField] protected float skillTimer;
    [SerializeField] protected Vector2 releaseDirection;
    [SerializeField] protected bool isCooling;
    protected SpriteRenderer playerSprite;

    protected virtual void Awake()
    {
        skillInputAction = new SkillInputAction();
    }

    protected virtual void Start()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        if (playerSprite == null) Debug.LogError("Player 身上未挂载 SpriteRenderer 组件，请检查预制体设置");

        if (skillInterval < 0f) Debug.LogError(id + "技能冷却时间未设置，请检查预制体设置");
    }

    protected virtual void OnEnable()
    {
        skillInputAction.Enable();
        skillInputAction.Normal.Direction.performed += GetDirection;
        skillInputAction.Normal.Direction.canceled += GetDirection;
    }

    protected virtual void OnDisable()
    {
        skillInputAction.Normal.Direction.performed -= GetDirection;
        skillInputAction.Normal.Direction.canceled -= GetDirection;
        skillInputAction.Disable();
    }

    protected virtual void GetDirection(InputAction.CallbackContext context)
    {
        releaseDirection = context.ReadValue<Vector2>();
        if (releaseDirection == Vector2.zero)
        {
            releaseDirection = playerSprite.flipX ? Vector2.left : Vector2.right;
        }
        releaseDirection = releaseDirection.normalized;
        // Debug.Log("玩家准备向 " + releaseDirection + " 方向释放技能");
    }

    public virtual void TryUseSkill()
    {

    }

    public virtual void TryUseSkill(SkillUseType useType)
    {

    }
}
