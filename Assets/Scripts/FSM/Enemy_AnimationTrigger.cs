using UnityEngine;

public class Enemy_AnimationTrigger : EntityAnimationTrigger
{
    private Enemy enemy;
    private Enemy_VFX enemy_VFX;
    protected override void Awake()
    {
        base.Awake();
        enemy=GetComponentInParent<Enemy>();
        enemy_VFX = GetComponentInParent<Enemy_VFX>();
    }
    public void EnableCounterWindow()
    {
        enemy_VFX.HandleAttackAlert(true);
        enemy.EnableCounterWindow(true);
    }
    public void DisableCounterWindow()
    {
        enemy_VFX.HandleAttackAlert(false);
        enemy.EnableCounterWindow(false);
    }
}
