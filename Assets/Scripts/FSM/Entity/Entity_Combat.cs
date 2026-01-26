using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public Collider2D[] targetColliders;

    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData damageScale;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;

    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    public void PerformAttack()
    {
        foreach (var collider in GetDetectedColliders())
        {
            IDamageable damageable = collider.GetComponent<IDamageable>();

            if (damageable == null) continue;

            ElementalEffectData effectData = new(stats, damageScale);

            float elementalDamage = stats.GetElementalDamage(out ElementType element,0.6f);
            bool isDamaged = damageable.TakeDamage(stats.GetPhysicalDamage(out bool isCrit), elementalDamage, element, transform);

            if(element!=ElementType.None)
                collider.GetComponent<Entity_StatusHandler>().ApplyEffect(element, effectData);


            if (isDamaged)
            {
                vfx?.CreateHitVFX(collider.transform, isCrit,element);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
