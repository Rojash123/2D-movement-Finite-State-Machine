using UnityEngine;

public class PlayerGroundedState : Entity
{
    public PlayerGroundedState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void Update()
    {
        base.Update();

        if(rb.linearVelocityY<0 && !player.isGrounded)
        {
            fsm.ChangeState(player.fallState);
        }

        if (player.InputAction.Player.Jump.WasPerformedThisFrame())
        {
            fsm.ChangeState(player.jumpState);
        }
        if (player.InputAction.Player.Attack.WasPerformedThisFrame())
        {
            fsm.ChangeState(player.attackState);
        }
    }
}
