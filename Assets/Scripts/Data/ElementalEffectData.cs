using System;
using UnityEngine;

[Serializable]
public class ElementalEffectData
{
    public float chillDuration;
    public float chillEffect;

    public float burnDuration;
    public float burnDamage;

    public float shockDuration;
    public float shockDamage;
    public float shockCharge;

    public ElementalEffectData(Entity_Stats stats, DamageScaleData damageScale)
    {
        chillDuration = damageScale.chillDuration;
        chillEffect = damageScale.chillSlowMultiplier;

        burnDuration = damageScale.burnDuration;
        burnDamage = stats.offense.fireDamage.GetValue * damageScale.burnDamageScale;

        shockDuration = damageScale.shockDuration;
        shockDamage = stats.offense.fireDamage.GetValue * damageScale.burnDamageScale;
        shockCharge = damageScale.shockCharge;
    }
}

public class ScaleFactor
{
    public float burnDamageScale;
}
