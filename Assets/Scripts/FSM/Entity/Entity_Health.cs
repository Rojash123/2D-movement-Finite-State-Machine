using System;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour, IDamageable
{
    private Slider healtBar;
    private Entity_VFX entityVfx;
    private Entity entity;
    private Entity_Stats entityStats;

    [SerializeField] protected float currentHp;
    [SerializeField] protected float maxHp;
    [SerializeField] protected bool isDead;

    [Header("OnDamageKnockBack")]
    public Vector2 knockBackPower = new(1.5f, 2.5f);
    public Vector2 heavyKnockBackPower = new(7, 7);
    public float knockBackDuration = 0.2f;
    public float heavyKnockBackDuration = 0.5f;

    [Header("Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;

    [Header("RegenHealth")]
    [SerializeField] private float regenInterval;
    [SerializeField] private bool canRegenHealth = true;

    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();
        healtBar = GetComponentInChildren<Slider>();
        entityStats = GetComponent<Entity_Stats>();
        maxHp = entityStats.GetMaxHealth();
        currentHp = maxHp;
        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0.1f,regenInterval);
    }
    private bool AttackEvaded() => UnityEngine.Random.Range(0, 100) < entityStats.GetEvasion();
    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead) return false;

        if (AttackEvaded()) return false;

        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;

        float mitigation = entityStats.GetArmorMitigation(armorReduction);
        float physicalFinalDamage = (1 - mitigation) * damage;

        float elementalRes = entityStats.GetEmentalResistance(element);
        float elementalfinalDamage = (1 - elementalRes) * elementalDamage;

        TakeKnockBack(damageDealer, physicalFinalDamage);
        ReduceHealth(physicalFinalDamage + elementalfinalDamage);

        return true;
    }
    private void TakeKnockBack(Transform damageDealer, float finalDamage)
    {
        Vector2 knockBack = CalculateKnockBack(damageDealer, finalDamage);
        float duration = CalculateDuration(finalDamage);
        entity?.ReceiveKnockBack(knockBack, duration);
    }
    public void RegenerateHealth()
    {
        if (!canRegenHealth) return;

        float regenAmount = entityStats.resource.healthRegen.GetValue;
        IncreaseHealth(regenAmount);
    }

    public float GetHealthPercentage() => currentHp / entityStats.GetMaxHealth();
    public void SetHealthPercentage(float percentage)
    {
        currentHp =Mathf.Clamp01(percentage) * entityStats.GetMaxHealth();
        UpdateHealthBar();
    }


    public void IncreaseHealth(float inceaseAmount)
    {
        if (isDead) return;

        currentHp += inceaseAmount;
        maxHp = entityStats.GetMaxHealth();
        currentHp = Mathf.Min(currentHp, maxHp);
        UpdateHealthBar();
    }
    public void ReduceHealth(float damage)
    {
        entityVfx?.PlayOnDamageVFX();
        currentHp -= damage;
        UpdateHealthBar();
        if (currentHp <= 0)
            Die();
    }
    private Vector2 CalculateKnockBack(Transform damageDealer, float damage)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        Vector2 knockBack = IsHeavyDamage(damage) ? heavyKnockBackPower : knockBackPower;
        knockBack.x *= direction;
        return knockBack;
    }
    private void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }
    private bool IsHeavyDamage(float damage) => damage / entityStats.GetMaxHealth() > heavyDamageThreshold;
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockBackDuration : knockBackDuration;
    private void UpdateHealthBar()
    {
        if (healtBar == null) return;

        healtBar.value = currentHp / entityStats.GetMaxHealth();
    }
}
