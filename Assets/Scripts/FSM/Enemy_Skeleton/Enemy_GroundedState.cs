using Unity.VisualScripting;
using UnityEngine;

public class Enemy_GroundedState : EnemyState
{
    public Enemy_GroundedState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName, enemy)
    {
    }
    public override void Update()
    {
        base.Update();
        if (enemy.PlayerDetection())
            fsm.ChangeState(enemy.battleState);
    }
}
