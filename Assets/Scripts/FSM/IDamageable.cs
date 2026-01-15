using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public interface IDamageable
{
    public void TakeDamage(float damage, Transform damageDealer);
}
