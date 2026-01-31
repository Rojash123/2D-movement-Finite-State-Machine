using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    [Header("Attack Upgrades")]
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;


    [Header("Attack Upgrades")]
    [SerializeField] private float damagePercentHealed = 0.3f;
    [SerializeField] private float coolDownReductionInSeconds;


    public float GetPercentageOfDamageHealted()
    {
        if (!ShouldCreateWisp()) return 0;
        return damagePercentHealed;
    }
    public float GetCoolDownReduction()
    {
        if (skillUpgradeType != SkillUpgradeType.TimeEcho_CoolDown_Wisp) return 0;
        return coolDownReductionInSeconds;
    }

    public bool CanReduceNegativeEffect()
    {
        return skillUpgradeType == SkillUpgradeType.TimeEcho_CleanWisp;
    }

    public bool ShouldCreateWisp()
    {
        return (skillUpgradeType==SkillUpgradeType.TimeEcho_HealWisp
            || skillUpgradeType == SkillUpgradeType.TimeEcho_CoolDown_Wisp
            || skillUpgradeType == SkillUpgradeType.TimeEcho_CleanWisp);
    }

    public float GetDuplicateChance()
    {
        if (skillUpgradeType != SkillUpgradeType.TimeEcho_ChanceToMultiply)
            return 0;

        return duplicateChance;
    }

    public int GetMaxAttack()
    {
        if (skillUpgradeType == SkillUpgradeType.TimeEcho_SingleAttack || skillUpgradeType == SkillUpgradeType.TimeEcho_ChanceToMultiply)
            return 1;

        if (skillUpgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
            return maxAttacks;

        return 0;
    }

    public override void TryUSeSkill()
    {
        if (!CanUseSkills()) return;
        CreateTimeEcho();
    }
    public float GetEchoTimeDuration()
    {
        return timeEchoDuration;
    }


    public void CreateTimeEcho(Vector3? targetPosition=null)
    {
        Vector3 position = targetPosition ?? transform.position;
        GameObject timeEcho = Instantiate(timeEchoPrefab, position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
