using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
        if (Unlocked(SkillUpgradeType.DashClone_OnStart)|| Unlocked(SkillUpgradeType.DashClone_OnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.DashShrad_OnStart) || Unlocked(SkillUpgradeType.DashShrad_OnStartAndArrival))
            CreateShrad();
    }
    public void OnEndEffect()
    {
        if (Unlocked(SkillUpgradeType.DashClone_OnStartAndArrival))
            CreateClone();

        if (Unlocked(SkillUpgradeType.DashShrad_OnStartAndArrival))
            CreateShrad();
    }
    private void CreateClone()
    {
    }
    private void CreateShrad()
    {
        skillManager.shard.CreateRawShard();
    }
}
