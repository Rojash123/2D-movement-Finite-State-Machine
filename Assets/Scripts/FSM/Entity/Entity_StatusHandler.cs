using System.Collections;
using UnityEngine;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVFX;
    private Entity_Stats stats;
    private Entity_Health entityHealth;

    private ElementType currentEffect = ElementType.None;

    [Header("Lightning Strike")]
    [SerializeField] private GameObject lightningVFX;
    private float currentCharge;
    private float maximumCharge = 1f;
    private Coroutine lightningCoroutine;

    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVFX = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
    }
    public void ApplyBurnEffect(float duration, float totalDamage)
    {
        float burnResistance = stats.GetEmentalResistance(ElementType.Fire);
        float reducedDuration = (1 - burnResistance) * duration;
        float reducedBurnDamage = (1 - burnResistance) * totalDamage;
        StartCoroutine(BurnEffect(reducedDuration, reducedBurnDamage));
    }
    private IEnumerator BurnEffect(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVFX.PlayOnStatusVFX(duration, currentEffect);

        int ticksPerSecond = 2;
        int totalTicks = Mathf.RoundToInt(duration * ticksPerSecond);

        float damagePerTick = totalDamage / totalTicks;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < totalTicks; i++)
        {
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        currentEffect = ElementType.None;

    }

    public void ApplyChilldedEffect(float duration, float slowMultiplier)
    {
        float iceResistance = stats.GetEmentalResistance(ElementType.Ice);
        float reducedDuration = (1 - iceResistance) * duration;

        StartCoroutine(ChillEffect(reducedDuration, slowMultiplier));
    }
    private IEnumerator ChillEffect(float duration, float slowMultiplier)
    {
        entity.SlowDownPlayer(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVFX.PlayOnStatusVFX(duration, currentEffect);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }

    public void ApplyElectrifyEffect(float duration, float totalDamage, float charge)
    {
        float lightningResistance = stats.GetEmentalResistance(ElementType.Lightning);
        float reducedCharge = (1 - lightningResistance) * charge;
        
        currentCharge += reducedCharge;

        if (currentCharge > maximumCharge)
        {
            InstantiateLightninfStrike(totalDamage);
            StopElectrifyEffect();
            return;
        }
        if (lightningCoroutine != null)
            StopCoroutine(lightningCoroutine);

        lightningCoroutine = StartCoroutine(LightningEffect(duration));
    }

    public void StopElectrifyEffect()
    {
        currentCharge = 0;
        entityVFX.StopAllVFX();
        currentEffect= ElementType.None;
    }
    private void InstantiateLightninfStrike(float totalDamage)
    {
        Instantiate(lightningVFX, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(totalDamage);
    }

    private IEnumerator LightningEffect(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVFX.PlayOnStatusVFX(duration, currentEffect);
        yield return new WaitForSeconds(duration);
        StopElectrifyEffect();
    }


    public bool canBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
            return true;

        return currentEffect == ElementType.None;
    }
}
