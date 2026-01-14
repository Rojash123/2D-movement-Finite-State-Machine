using UnityEngine;

public class Enemy_AttackState : EnemyState
{
    public Enemy_AttackState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
    }
    public override void Update()
    {
        base.Update();
        if (triggerCalled)
            fsm.ChangeState(enemy.battleState);
    }
}
