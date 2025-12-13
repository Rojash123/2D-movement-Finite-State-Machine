using UnityEngine;

public class FiniteStateMachine
{
    public Entity currentState {  get; private set; }

    public void InititalizeMethod(Entity state)
    {
        currentState = state;
        currentState.EnterState();
    }

    public void ChangeState(Entity newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
    
}
