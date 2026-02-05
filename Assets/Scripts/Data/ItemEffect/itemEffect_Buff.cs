using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect-Buff", menuName = "Scriptable Objects/BuffEffect")]

public class itemEffect_Buff : ItemEffectDataSO
{
    [SerializeField] private BuffEffectData[] buff;
    [SerializeField] private float duration = 1f;
    [SerializeField] private string source = Guid.NewGuid().ToString();

    private Player_Stats stats;

    public override bool CanbeUsed()
    {
        if (stats == null)
            stats = FindAnyObjectByType<Player_Stats>();

        return stats.CanApplyBuffOn(source);
    }
    public override void ExecuteEffect()
    {
        stats.ApplyBuff(buff, duration, source);
    }
}
