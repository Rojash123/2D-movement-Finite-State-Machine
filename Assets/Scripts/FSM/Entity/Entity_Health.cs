using System;
using UnityEngine;

public class Entity_Health : MonoBehaviour,IDamageable
{
    private Entity_VFX entityVfx;
    private Entity entity;

    [SerializeField] protected float maxHealth = 100f;
    [SerializeField] protected float currentHp;
    [SerializeField] protected bool isDead;

    [Header("OnDamageKnockBack")]
    public Vector2 knockBackPower = new(1.5f, 2.5f);
    public Vector2 heavyKnockBackPower = new(7, 7);
    public float knockBackDuration = 0.2f;
    public float heavyKnockBackDuration = 0.5f;

    [Header("Heavy Damage")]
    [SerializeField] private float heavyDamageThreshold = 0.3f;


    protected virtual void Awake()
    {
        entityVfx = GetComponent<Entity_VFX>();
        entity = GetComponent<Entity>();

        currentHp = maxHealth;
    }

    public virtual void TakeDamage(float damage, Transform damageDealer)
    {
        if (isDead) return;

        Vector2 knockBack = CalculateKnockBack(damageDealer, damage);
        float duration = CalculateDuration(damage);
        entityVfx?.PlayOnDamageVFX();
        entity?.ReceiveKnockBack(knockBack, duration);

        ReduceHP(damage);
    }
    protected void ReduceHP(float damage)
    {
        currentHp -= damage;
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
    private bool IsHeavyDamage(float damage) => damage / maxHealth > heavyDamageThreshold;
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockBackDuration : knockBackDuration;

}
