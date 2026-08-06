using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Blow : Skill
{
    [SerializeField] private GameObject weakWind;
    [SerializeField] private GameObject strongWind;

    public bool test = true; // TODO: 测试用，记得删除

    protected override void Awake()
    {
        base.Awake();
        id = SkillId.Blow;
    }

    protected override void Start()
    {
        base.Start();

        if (SkillsManager.Instance != null) SkillsManager.Instance.RegisterSkill(this, false);
        else Debug.LogError("SkillsManager 不存在实例");
        Debug.Log("注册了 Blow 技能，当前技能解锁状态为: " + SkillsManager.Instance.IsUnlocked(id));

        if (weakWind == null) Debug.LogError("技能弱风预制体未设置，请检查玩家预制体设置");
        if (strongWind == null) Debug.LogError("技能强风预制体未设置，请检查玩家预制体设置");

    }

    protected override void OnEnable()
    {
        base.OnEnable();
        skillInputAction.Normal.Blow.performed += OnBlowPerformed;
        

    }
    protected override void OnDisable()
    {
        if (SkillsManager.Instance != null) SkillsManager.Instance.UnregisterSkill(this);
        else Debug.LogError("SkillsManager 不存在实例");
        Debug.Log("注销了 Blow 技能");

        skillInputAction.Normal.Blow.performed -= OnBlowPerformed;
        base.OnDisable();
    }

    void Update()
    {
        RefreshCoolTime();

        // TODO: 测试用，记得删除
        if (test && SkillsManager.Instance != null && !SkillsManager.Instance.IsUnlocked(id))
        {
            SkillsManager.Instance.UnlockSkill(id);
        }

    }

    public override void TryUseSkill(SkillUseType useType)
    {
        switch (useType)
        {
            case SkillUseType.weak:
                GameObject weakWindBall = Instantiate(weakWind, transform.position, Quaternion.identity);
                weakWindBall.GetComponent<WindBall>().Init(releaseDirection);
                break;
            case SkillUseType.strong:
                GameObject StrongWindBall = Instantiate(strongWind, transform.position, Quaternion.identity);
                StrongWindBall.GetComponent<WindBall>().Init(releaseDirection);
                break;
            default:
                Debug.LogError("未知的技能使用类型: " + useType);
                break;
        }
        isCooling = true;
    }

    private void OnBlowPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (SkillsManager.Instance == null) return;
        if (!SkillsManager.Instance.IsUnlocked(id) || isCooling) return;

        if (context.interaction is HoldInteraction)
        {
            Debug.Log("玩家长按了 Blow 技能释放按钮，尝试释放强风技能");
            TryUseSkill(SkillUseType.strong);
        }
        else if (context.interaction is TapInteraction)
        {
            Debug.Log("玩家点击了 Blow 技能释放按钮，尝试释放弱风技能");
            TryUseSkill(SkillUseType.weak);
        }
    }

    private void RefreshCoolTime()
    {
        if (!isCooling) return;

        skillTimer += Time.deltaTime;
        if (skillTimer >= skillInterval)
        {
            isCooling = false;
            skillTimer = 0f;
        }
    }
}
