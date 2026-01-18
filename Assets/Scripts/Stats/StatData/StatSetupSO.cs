using UnityEngine;

[CreateAssetMenu(fileName = "StatSetupSO", menuName = "Scriptable Objects/StatSetupSO")]
public class StatSetupSO : ScriptableObject
{
    [Header("Resources")]
    public float maxHealth;
    public float healthRegen;

    [Header("Major")]
    public float aligity;
    public float strength;
    public float vitality;
    public float intelligence;

    [Header("Offense")]
    public float attackSpeed;
    public float damage;
    public float criticalPower;
    public float critChance;
    public float armorReduction;
    public float fireDamage;
    public float iceDamage;
    public float lightningDamage;

    [Header("Defence")]
    public float armor;
    public float evasion;
    public float fireRes;
    public float iceRes;
    public float lightningRes;
}
