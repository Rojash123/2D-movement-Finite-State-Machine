using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{
    private Entity entity;
    private Entity_Combat entityCombat;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
        entityCombat = GetComponentInParent<Entity_Combat>();
    }
    private void AttackOver()
    {
        entity.CallAnimationTrigger();
    }
    private void AttackTrigger()
    {
        entityCombat.PerformAttack();
    }
}
