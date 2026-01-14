using UnityEngine;

public abstract class EntityState
{
    protected FiniteStateMachine fsm;
    protected string animBoolName;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(FiniteStateMachine finiteStateMachine, string stateName)
    {
        this.animBoolName = stateName;
        fsm = finiteStateMachine;
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
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameter();
    }
    public virtual void ExitState()
    {
        anim.SetBool(animBoolName, false);
    }
    public virtual void UpdateAnimationParameter()
    {

    }
}
