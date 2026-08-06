using System.Collections.Generic;
using UnityEngine;

public enum SkillId
{
    Blow,
}

public class SkillsManager : MonoBehaviour
{
    public static SkillsManager Instance;

    private readonly Dictionary<SkillId, Skill> skills = new();
    private readonly Dictionary<SkillId, bool> skillUnlocked = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    public void RegisterSkill(Skill skill, bool unlocked)
    {
        skills[skill.Id] = skill;
        skillUnlocked[skill.Id] = unlocked;
    }

    public void UnregisterSkill(Skill skill)
    {
        if (skills.TryGetValue(skill.Id, out Skill registeredSkill) &&
            ReferenceEquals(registeredSkill, skill))
        {
            skills.Remove(skill.Id);
        }
    }

    public void UnlockSkill(SkillId id)
    {
        skillUnlocked[id] = true;
    }

    public void LockSkill(SkillId id)
    {
        skillUnlocked[id] = false;
    }

    public bool IsUnlocked(SkillId id)
    {
        return skillUnlocked.TryGetValue(id, out bool unlocked) && unlocked;
    }

    public void TryUseSkill(SkillId id, SkillUseType useType)
    {
        if (!IsUnlocked(id))
            return;

        if (skills.TryGetValue(id, out Skill skill))
        {
            skill.TryUseSkill(useType);
        }
    }
}
