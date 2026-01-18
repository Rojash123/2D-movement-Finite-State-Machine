using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public interface IDamageable
{
    public bool TakeDamage(float damage,float elementalDamage,ElementType element,Transform damageDealer);
}
