using UnityEngine;

public class IdleState : PlayerGroundedState
{
    public IdleState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }

    public override void EnterState()
    {
        base.EnterState();
        player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocityY);
    }
    public override void Update()
    {
        base.Update();
        HandleMove();
    }
    void HandleMove()
    {
        if (player.moveVector.x != 0)
        {
            fsm.ChangeState(player.moveState);
        }
    }
}
