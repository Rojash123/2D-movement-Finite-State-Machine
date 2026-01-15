using UnityEngine;

public class Enemy_Stunned : EnemyState
{
    public Enemy_Stunned(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
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
