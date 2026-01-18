using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public Collider2D[] targetColliders;

    private Entity_VFX vfx;
    private Entity_Stats stats;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;


    [Header("Status Effect Details")]
    [SerializeField] private float defaultDuration = 3f;
    [SerializeField] private float chillSlowMultiplier = 0.2f;
    [SerializeField] private float chargeBuildUp = 0.3f;
    [Space]
    [SerializeField] private float fireScaleFactor = 0.8f;
    [SerializeField] private float lightningScaleFactor = 2.5f;



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

            float elementalDamage = stats.GetElementalDamage(out ElementType element,0.6f);
            bool isDamaged = damageable.TakeDamage(stats.GetPhysicalDamage(out bool isCrit), elementalDamage, element, transform);

            if (element != ElementType.None)
                ApplyStatusEffects(collider.transform, element);

            if (isDamaged)
            {
                vfx?.UpdateOnHitColor(element);
                vfx?.CreateHitVFX(collider.transform, isCrit);
            }
        }
    }

    public void ApplyStatusEffects(Transform target, ElementType element,float scaleFactor=1)
    {
        Entity_StatusHandler status = target.GetComponent<Entity_StatusHandler>();

        if (status == null) return;


        if (element == ElementType.Ice && status.canBeApplied(element))
            status.ApplyChilldedEffect(defaultDuration, chillSlowMultiplier);

        if (element == ElementType.Fire && status.canBeApplied(element))
        {
            scaleFactor = fireScaleFactor;
            float fireDamage = stats.offense.fireDamage.GetValue*scaleFactor;
            status.ApplyBurnEffect(defaultDuration, fireDamage);
        }

        if (element == ElementType.Lightning && status.canBeApplied(element))
        {
            scaleFactor = lightningScaleFactor;
            float lightningDamage = stats.offense.lightningDamage.GetValue * scaleFactor;
            status.ApplyElectrifyEffect(defaultDuration, lightningDamage, chargeBuildUp);
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
