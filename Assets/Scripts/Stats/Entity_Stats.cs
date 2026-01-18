using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Entity_Stats : MonoBehaviour
{
    [SerializeField] private StatSetupSO statDefaultData;

    public ElementType type;
    public Stat_ResourcesGroup resource;

    public Stat_MajorGroup major;
    public Stat_OffensiveGroup offense;
    public Stat_DefensiveGroup defense;

    public float GetMaxHealth()
    {
        float baseHp = resource.maxHealth.GetValue;
        float bonusHp = major.vitality.GetValue * 5;
        float totalHealth = baseHp + bonusHp;
        return totalHealth;
    }
    public float GetPhysicalDamage(out bool isCrit, float scaleFactor = 1)
    {
        float baseDamage = offense.damage.GetValue;
        float bonusDamage = major.strength.GetValue;

        float totalBaseDamage = baseDamage + bonusDamage;

        float baseCritChance = offense.critChance.GetValue;
        float bonusCritChance = major.aligity.GetValue * 0.3f;
        float critChance = baseCritChance + bonusCritChance;

        float baseCritPower = offense.criticalPower.GetValue;
        float bonusCritPower = major.strength.GetValue * 0.5f;
        float critPower = (baseCritPower + bonusCritPower) / 100;

        isCrit = Random.Range(0, 100) < critChance;

        float finalDamage = isCrit ? totalBaseDamage * critPower : totalBaseDamage;
        return finalDamage*scaleFactor;
    }
    public float GetElementalDamage(out ElementType element,float scaleFactor=1)
    {
        float fireDamage = offense.fireDamage.GetValue;
        float iceDamage = offense.iceDamage.GetValue;
        float lightningDamage = offense.lightningDamage.GetValue;

        float bonusElementalDamage = major.intelligence.GetValue;

        float highestDamage = fireDamage;
        element = ElementType.Fire;

        if (iceDamage > highestDamage)
        {
            highestDamage = iceDamage;
            element = ElementType.Ice;
        }

        if (lightningDamage > highestDamage)
        {
            highestDamage = lightningDamage;
            element = ElementType.Lightning;
        }

        if (highestDamage <= 0)
        {
            element = ElementType.None;
            return 0;
        }

        float bonusFire = element == ElementType.Fire ? 0 : fireDamage * 0.5f;
        float bonusIce = element == ElementType.Ice ? 0 : iceDamage * 0.5f;
        float bonusLightning = element == ElementType.Lightning ? 0 : lightningDamage * 0.5f;

        float weakerElementDamage = bonusFire + bonusIce + bonusLightning;

        float finalDamage = highestDamage + bonusElementalDamage + weakerElementDamage;
        return finalDamage*scaleFactor;
    }

    public float GetArmorMitigation(float reduction)
    {
        float baseArmor = defense.armor.GetValue;
        float bonusArmor = major.vitality.GetValue;
        float totalArmor = baseArmor + bonusArmor;

        float actualArmor = totalArmor * Mathf.Clamp01(1 - reduction);

        float mitigation = actualArmor / (100 + actualArmor);
        mitigation = Mathf.Clamp(mitigation, 0, 0.85f);
        return mitigation;
    }
    public float GetEmentalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = major.intelligence.GetValue * 0.5f;

        baseResistance = element switch
        {
            ElementType.Fire => defense.fireRes.GetValue,
            ElementType.Ice => defense.iceRes.GetValue,
            ElementType.Lightning=> defense.lightningRes.GetValue,
            _ => 0,
        };
        float resistance = baseResistance + bonusResistance;
        float resitanceCap = 75f;
        resistance = Mathf.Clamp(resistance, 0, resitanceCap)/100;
        return resistance;
    }
    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue;
        float bonusEvasion = major.aligity.GetValue * .5f;

        return Mathf.Clamp(baseEvasion + bonusEvasion, 0, 85f);
    }
    public float GetArmorReduction()
    {
        float reduction = offense.armorReduction.GetValue / 100;
        return reduction;
    }
    public Stat GetStat(StatType type)
    {
        switch (type)
        {
            case StatType.maxHealth:return resource.maxHealth;
            case StatType.healthRegen: return resource.healthRegen;
            
            case StatType.Agility: return major.aligity;
            case StatType.Vitality: return major.vitality;
            case StatType.Intelligence: return major.intelligence;
            case StatType.Strength: return major.strength;
            
            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CriticalPower: return offense.criticalPower;
            case StatType.CriticalChance: return offense.critChance;
            case StatType.ArmorReduction: return offense.armorReduction;

            case StatType.FireDamage: return offense.fireDamage;
            case StatType.IceDamage: return offense.iceDamage;
            case StatType.LightningDamage: return offense.lightningDamage;

            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;
            case StatType.FireResistance: return defense.fireRes;
            case StatType.IceResistance: return defense.iceRes;
            case StatType.lightningResistance: return defense.lightningRes;
                
            default: return null;
        }
    }

    [ContextMenu("Apply Default Value")]
    public void SetInitialStatData()
    {
        if (statDefaultData == null) return;

        resource.maxHealth.SetBaseValue(statDefaultData.maxHealth);
        resource.healthRegen.SetBaseValue(statDefaultData.healthRegen);

        major.strength.SetBaseValue(statDefaultData.strength);
        major.vitality.SetBaseValue(statDefaultData.vitality);
        major.aligity.SetBaseValue(statDefaultData.aligity);
        major.intelligence.SetBaseValue(statDefaultData.intelligence);

        offense.attackSpeed.SetBaseValue(statDefaultData.attackSpeed);
        offense.damage.SetBaseValue(statDefaultData.damage);
        offense.criticalPower.SetBaseValue(statDefaultData.criticalPower);
        offense.critChance.SetBaseValue(statDefaultData.critChance);
        offense.armorReduction.SetBaseValue(statDefaultData.armorReduction);
        offense.fireDamage.SetBaseValue(statDefaultData.fireDamage);
        offense.iceDamage.SetBaseValue(statDefaultData.iceDamage);
        offense.lightningDamage.SetBaseValue(statDefaultData.lightningDamage);

        defense.armor.SetBaseValue(statDefaultData.armor);
        defense.evasion.SetBaseValue(statDefaultData.evasion);
        defense.fireRes.SetBaseValue(statDefaultData.fireRes);
        defense.iceRes.SetBaseValue(statDefaultData.iceRes);
        defense.lightningRes.SetBaseValue(statDefaultData.lightningRes);
    }
}
