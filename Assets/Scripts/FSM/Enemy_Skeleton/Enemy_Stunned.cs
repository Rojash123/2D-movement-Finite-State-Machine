using UnityEngine;

public class Enemy_Stunned : EnemyState
{
    private Enemy_VFX enemy_VFX;

    public Enemy_Stunned(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
        enemy_VFX = enemy.GetComponent<Enemy_VFX>();
    }
    public override void EnterState()
    {
        base.EnterState();
        enemy_VFX.HandleAttackAlert(false);
        stateTimer = enemy.stunnedDuration;
        rb.linearVelocity = new Vector2(enemy.stunnedVelocity.x * -enemy.facingDir, enemy.stunnedVelocity.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
            fsm.ChangeState(enemy.idleState);
    }
}
