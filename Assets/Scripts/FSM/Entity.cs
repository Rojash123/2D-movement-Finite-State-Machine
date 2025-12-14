using UnityEngine;

public abstract class Entity
{
    protected FiniteStateMachine fsm;
    protected string animBoolName;
    protected Player player;

    protected Animator anim;
    protected Rigidbody2D rb;

    protected float stateTimer;
    protected bool triggerCalled;
    public Entity(FiniteStateMachine finiteStateMachine,string stateName, Player player)
    {
        this.animBoolName = stateName;
        fsm = finiteStateMachine;
        this.player = player;
        anim = player.animator;
        rb = player.rb;
    }
    public virtual void EnterState()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public void CallAnimationTrigger()
    {
        triggerCalled = true;
    }
    public virtual void Update()
    {
        stateTimer-= Time.deltaTime;
        anim.SetFloat("yVelocity",rb.linearVelocityY);

        if (player.InputAction.Player.Dash.WasPerformedThisFrame() && canDash())
        {
            fsm.ChangeState(player.dashState);
        }
    }
    private bool canDash()
    {
        if (player.isWallDetected) return false;
        if (fsm.currentState == player.dashState) return false;
        return true;
    }
    public virtual void ExitState()
    {
        anim.SetBool(animBoolName, false);
    }
}
