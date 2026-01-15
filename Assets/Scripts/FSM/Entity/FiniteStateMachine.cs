using UnityEngine;

public class FiniteStateMachine
{
    public bool canChangeState;
    public EntityState currentState {  get; private set; }

    public void InititalizeMethod(EntityState state)
    {
        canChangeState = true;
        currentState = state;
        currentState.EnterState();
    }

    public void ChangeState(EntityState newState)
    {
        if (!canChangeState)
            return;

        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    public void SwitchOffStateMachine() => canChangeState = false;
    
}
