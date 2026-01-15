using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    [SerializeField] private float damage;
    public Collider2D[] targetColliders;

    [Header("Target Detection")]
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius;
    [SerializeField] private LayerMask whatIsTarget;

    private Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }

    public void PerformAttack()
    {
        foreach (var collider in GetDetectedColliders())
        {
            IDamageable targetHealth = collider.GetComponent<IDamageable>();
            targetHealth?.TakeDamage(damage, transform);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
