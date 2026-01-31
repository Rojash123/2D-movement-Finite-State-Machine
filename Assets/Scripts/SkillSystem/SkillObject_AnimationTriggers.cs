using UnityEngine;

public class SkillObject_AnimationTriggers : MonoBehaviour
{
    private SkillObject_TimeEcho timeEcho;
    private void Awake()
    {
        timeEcho = GetComponentInParent<SkillObject_TimeEcho>();
    }

    private void AttackTrigger()
    {
        timeEcho.PerformAttacK();
    }

    private void TryTerminateAttack(int currentAttackIndex)
    {
        if (currentAttackIndex == timeEcho.maxAttack)
            timeEcho.HandleDeath();
    }
}
