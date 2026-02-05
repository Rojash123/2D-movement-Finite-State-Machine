using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData_SO", menuName = "Scriptable Objects/SkillData_SO")]
public class SkillData_SO : ScriptableObject
{
    public int cost;
    public bool unlockByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;

    [Header("Skill Description")]
    public string skillName;
    [TextArea]
    public string Description;
    public Sprite skillIcon;
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType skillUpgradeType;
    public float coolDown;
    public DamageScaleData damageScale;
}
