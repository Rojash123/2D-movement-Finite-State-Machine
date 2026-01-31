using System;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject OnHitVfx;

    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1f;
    protected Rigidbody2D rb;

    protected ElementType element;
    protected Animator anim;
    protected Entity_Stats stats;
    protected DamageScaleData damageScaleData;
    protected bool targetGotHit;
    protected Transform lastTarget;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected void DamageEnemyInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            if (damageable == null) continue;

            ElementalEffectData effectData = new(stats, damageScaleData);

            float physicalDamage = stats.GetPhysicalDamage(out bool isCrit, damageScaleData.physical);
            float elementalDamage = stats.GetElementalDamage(out ElementType element, damageScaleData.elemental);


            if (element != ElementType.None)
                target.GetComponent<Entity_StatusHandler>().ApplyEffect(element, effectData);

            targetGotHit = damageable.TakeDamage(physicalDamage, elementalDamage, element, transform);
            this.element = element;

            if (targetGotHit)
            {
                lastTarget = target.transform;
                Instantiate(OnHitVfx, transform.position, Quaternion.identity);
            }
        }
    }

    protected Transform ClosestTarget()
    {
        Transform target = null;
        float closestDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, 10f))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                target = enemy.transform;
                closestDistance = distance;
            }
        }
        return target;
    }

    protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    private void OnDrawGizmos()
    {
        if (targetCheck == null) targetCheck = transform;

        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
