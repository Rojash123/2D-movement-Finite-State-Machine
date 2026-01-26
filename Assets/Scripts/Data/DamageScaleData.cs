using System;
using UnityEngine;

[Serializable]
public class DamageScaleData
{
    [Header("Damage Details")]
    public float physical = 1;
    public float elemental = 1;

    [Header("Chill Element Details")]
    public float chillDuration = 3f;
    public float chillSlowMultiplier = 0.2f;

    [Header("Burn Element Details")]
    public float burnDuration = 3;
    public float burnDamageScale = 1;

    [Header("Shock Element Details")]
    public float shockDuration = 3;
    public float shockDamageScale = 1;
    public float shockCharge = 0.4f;
}
