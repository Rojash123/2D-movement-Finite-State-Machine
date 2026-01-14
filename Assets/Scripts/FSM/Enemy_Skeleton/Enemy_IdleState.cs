using UnityEngine;

public class Enemy_IdleState : Enemy_GroundedState
{
    public Enemy_IdleState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {

    }

    public override void EnterState()
    {
        base.EnterState();
        stateTimer = enemy.idleTime;
    }
    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            fsm.ChangeState(enemy.moveState);
        }
    }
    public override void ExitState()
    {
        base.ExitState();
    }
}
