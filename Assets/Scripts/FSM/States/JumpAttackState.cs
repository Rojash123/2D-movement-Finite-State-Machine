using UnityEngine;

public class JumpAttackState : Entity
{
    private bool touchGround;
    public JumpAttackState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
        fsm = finiteStateMachine;
        this.animBoolName = stateName;
        this.player = player;
    }
    public override void EnterState()
    {
        base.EnterState();
        player.SetVelocity(player.jumpAttackVelocity.x * player.facingDir, player.jumpAttackVelocity.y);
        touchGround = false;
    }
    public override void Update()
    {
        base.Update();
        if (player.isGrounded && !touchGround) 
        {
            touchGround = true;
            anim.SetTrigger("jumpattackTrigger");
            player.SetVelocity(0, rb.linearVelocityY);
        }

        if(player.isGrounded && triggerCalled)
        {
            fsm.ChangeState(player.idleState);
        }

    }
}
