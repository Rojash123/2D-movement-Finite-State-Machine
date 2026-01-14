using UnityEngine;

public class EntityAnimationTrigger : MonoBehaviour
{

    private Entity entity;

    private void Awake()
    {
        entity = GetComponentInParent<Entity>();
    }
    private void AttackOver()
    {
        entity.CallAnimationTrigger();
    }
}
