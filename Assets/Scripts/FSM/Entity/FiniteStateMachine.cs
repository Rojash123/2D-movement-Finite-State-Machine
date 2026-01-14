using UnityEngine;

public class FiniteStateMachine
{
    public EntityState currentState {  get; private set; }

    public void InititalizeMethod(EntityState state)
    {
        currentState = state;
        currentState.EnterState();
    }

    public void ChangeState(EntityState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    
}
