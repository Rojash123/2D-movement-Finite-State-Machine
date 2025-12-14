using UnityEngine;

public class PlayerWallSlideState : Entity
{
    public PlayerWallSlideState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }

    public override void EnterState()
    {
        base.EnterState();
    }
    public override void Update()
    {
        base.Update();
        wallSlide();
        if (player.InputAction.Player.Jump.WasPerformedThisFrame())
        {
            fsm.ChangeState(player.walljumpState);
        }
        if (!player.isWallDetected) 
        {
            fsm.ChangeState(player.fallState);
        }
        if (player.isGrounded)
        {
            player.Flip();
            fsm.ChangeState(player.idleState);
        }
    }
    private void wallSlide()
    {
        if (player.moveVector.y < 0)
        {
            player.SetVelocity(player.moveVector.x, rb.linearVelocityY);
        }
        else
        {
            player.SetVelocity(player.moveVector.x, rb.linearVelocityY*player.wallSlidemultiplier);
        }
    }
}
