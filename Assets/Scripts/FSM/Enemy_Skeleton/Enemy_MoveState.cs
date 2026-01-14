using UnityEngine;

public class Enemy_MoveState : Enemy_GroundedState
{
    public Enemy_MoveState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        if (!enemy.isGroundCheck || enemy.isWallDetected)
            enemy.Flip();
    }
    public override void Update()
    {
        base.Update();
        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, rb.linearVelocity.y);
        if (!enemy.isGroundCheck || enemy.isWallDetected)
            fsm.ChangeState(enemy.idleState);
    }
    public override void ExitState()
    {
        base.ExitState();
    }

}
