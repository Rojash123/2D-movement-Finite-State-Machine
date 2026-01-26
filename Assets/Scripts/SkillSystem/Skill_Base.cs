using UnityEngine;

public class Skill_Base : MonoBehaviour
{
    public Player_SkillManager skillManager {  get; private set; }
    public Player player {  get; private set; }
    public DamageScaleData damageScaleData {  get; private set; }

    [Header("General Details")]
    [SerializeField] protected float coolDown;
    private float lastTimeUsed;

    [SerializeField] protected SkillType skillType;
    [SerializeField] protected SkillUpgradeType skillUpgradeType;

    protected virtual void Awake()
    {
        player = GetComponentInParent<Player>();
        skillManager = GetComponentInParent<Player_SkillManager>();
        lastTimeUsed -= coolDown;
    }
    public virtual void TryUSeSkill()
    {

    }
    public void SetSkillUpgrade(UpgradeData data)
    {
        skillUpgradeType = data.skillUpgradeType;
        coolDown = data.coolDown;
        damageScaleData = data.damageScale;
    }

    protected bool Unlocked(SkillUpgradeType type) => skillUpgradeType == type;

    public bool CanUseSkills()
    {
        if(skillUpgradeType==SkillUpgradeType.None) return false;

        if (OnCooldown()) return false;

        return true;
    }
    protected bool OnCooldown() => Time.time < lastTimeUsed + coolDown;
    public void SetSkillOnCoolDown() => lastTimeUsed = Time.time;
    public void ReduceCoolDownBY(float reduceCooldownDuration) => lastTimeUsed += reduceCooldownDuration;
    public void ResetCoolDown() => lastTimeUsed = Time.time;

}
