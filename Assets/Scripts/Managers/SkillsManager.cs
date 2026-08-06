using System.Collections.Generic;
using UnityEngine;

public enum SkillId
{
    Blow,
}

public class SkillsManager : MonoBehaviour
{
    public static SkillsManager Instance;

    private Dictionary<SkillId, Skill> skills;
    private Dictionary<SkillId, bool> skillUnlocked;

    private void Awake()
    {

        skills = new Dictionary<SkillId, Skill>();
        skillUnlocked = new Dictionary<SkillId, bool>();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public void RegisterSkill(Skill skill, bool unlocked)
    {
        skills[skill.id] = skill;
        skillUnlocked[skill.id] = unlocked;
    }

    public void UnregisterSkill(Skill skill)
    {
        if (skills.TryGetValue(skill.id, out Skill registeredSkill) &&
            ReferenceEquals(registeredSkill, skill))
        {
            skills.Remove(skill.id);
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
}
