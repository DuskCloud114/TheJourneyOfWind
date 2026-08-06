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
    public SkillId Id { get; }
    [SerializeField] protected SkillInputAction skillInputAction = new SkillInputAction();
    [SerializeField] protected SkillUseType skillUseType;
    [SerializeField] protected float skillInterval;
    [SerializeField] protected float skillTimer;
    [SerializeField] protected Vector2 releaseDirection;
    [SerializeField] protected bool isUsingSkill;


    protected virtual void OnEnable()
    {
        skillInputAction.Enable();
    }

    protected virtual void OnDisable()
    {
        skillInputAction.Disable();
    }

    

    public virtual void TryUseSkill()
    {
        
    }

    public virtual void TryUseSkill(SkillUseType useType)
    {
        
    }
}
