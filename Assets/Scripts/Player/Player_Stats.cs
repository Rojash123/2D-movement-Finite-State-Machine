using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Player_Stats : Entity_Stats
{
    private List<string> activeBuffs = new List<string>();
    private Inventory_Player inventory;

    protected override void Awake()
    {
        base.Awake();
        inventory = GetComponent<Inventory_Player>();
    }

    public bool CanApplyBuffOn(string source)
    {
        return !activeBuffs.Contains(source);
    }
    public void ApplyBuff(BuffEffectData[] buffToApply, float duration, string source)
    {
        StartCoroutine(ApplyBuffCO(buffToApply, duration, source));
    }

    private IEnumerator ApplyBuffCO(BuffEffectData[] buffToApply, float duration, string source)
    {
        activeBuffs.Add(source);

        foreach (var buff in buffToApply)
        {
            GetStat(buff.StatType).AddModifier(buff.value, source);
        }
        yield return new WaitForSeconds(duration);
        foreach (var buff in buffToApply)
        {
            GetStat(buff.StatType).RemoveModifier(source);
        }
        activeBuffs.Remove(source);
        inventory.TriggerUIUpdate();
    }
}
