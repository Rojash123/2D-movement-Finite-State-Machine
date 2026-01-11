using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    public EnemyState(FiniteStateMachine finiteStateMachine, string stateName, Enemy enemy) : base(finiteStateMachine, stateName)
    {
        this.enemy = enemy;
        anim = enemy.animator;
        rb = enemy.rb;
    }
}
