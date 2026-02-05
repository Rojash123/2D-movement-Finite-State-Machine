using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private RectTransform rect;
    private UI ui;
    private Player_Stats playerStats;

    [SerializeField] private StatType statType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindAnyObjectByType<Player_Stats>();
    }

    public void UpdateStatValue()
    {
        Stat stat = playerStats.GetStat(statType);

        if (stat == null && statType!=StatType.ElementalDamage) return;

        float value = 0;

        switch (statType)
        {
            //MAJOR

            case StatType.Strength:
                value = playerStats.major.strength.GetValue;
                break;

            case StatType.Agility:
                value = playerStats.major.aligity.GetValue;
                break;

            case StatType.Intelligence:
                value = playerStats.major.intelligence.GetValue;
                break;

            case StatType.Vitality:
                value = playerStats.major.vitality.GetValue;
                break;

            //OFFENSE

            case StatType.Damage:
                value = playerStats.GetBaseDamage();
                break;

            case StatType.CriticalChance:
                value = playerStats.GetCritChance();
                break;

            case StatType.CriticalPower:
                value = playerStats.GetCritPower();
                break;

            case StatType.ArmorReduction:
                value = playerStats.GetArmorReduction() * 100;
                break;

            case StatType.AttackSpeed:
                value = playerStats.offense.attackSpeed.GetValue * 100f;
                break;

            //DEFENSE

            case StatType.maxHealth:
                value = playerStats.GetMaxHealth();
                break;

            case StatType.healthRegen:
                value = playerStats.resource.healthRegen.GetValue;
                break;

            case StatType.Evasion:
                value = playerStats.GetEvasion();
                break;

            case StatType.Armor:
                value = playerStats.GetBaseArmor();
                break;

            //Elemental Damage
            case StatType.FireDamage:
                value = playerStats.offense.fireDamage.GetValue;
                break;

            case StatType.IceDamage:
                value = playerStats.offense.iceDamage.GetValue;
                break;

            case StatType.LightningDamage:
                value = playerStats.offense.lightningDamage.GetValue;
                break;

            case StatType.ElementalDamage:
                value = playerStats.GetElementalDamage(out ElementType element,1);
                break;

            //Elemental Resistamce
            case StatType.FireResistance:
                value = playerStats.GetEmentalResistance(ElementType.Fire)*100;
                break;

            case StatType.IceResistance:
                value = playerStats.GetEmentalResistance(ElementType.Ice)*100;
                break;

            case StatType.lightningResistance:
                value = playerStats.GetEmentalResistance(ElementType.Lightning) * 100;
                break;
        }

        statValue.text = IsPercentage(statType)?value+"%":value.ToString();
    }

    private bool IsPercentage(StatType type)
    {
        switch (type)
        {
            case StatType.CriticalChance:
            case StatType.CriticalPower:
            case StatType.IceResistance:
            case StatType.FireResistance:
            case StatType.lightningResistance:
            case StatType.AttackSpeed:
            case StatType.Evasion:
                return true;

            default: return false;
        }
    }

    private void OnValidate()
    {
        gameObject.name = "UI_Stat" + statType.ToString();
        statName.text = statType.ToString();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect, statType);
    }
}
