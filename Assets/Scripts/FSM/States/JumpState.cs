using UnityEngine;

public class JumpState : PlayerAirState
{
    public JumpState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }

    public override void EnterState()
    {
        player.JumpPlayer();
        base.EnterState();
    }
    public override void Update()
    {
        base.Update();
        if (player.rb.linearVelocityY <= 0)
        {
            fsm.ChangeState(player.fallState);
        }
    }
}
