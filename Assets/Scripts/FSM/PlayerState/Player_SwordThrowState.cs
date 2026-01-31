using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    private Camera mainCamera;
    public Player_SwordThrowState(FiniteStateMachine finiteStateMachine, string stateName, Player player) : base(finiteStateMachine, stateName, player)
    {
    }

    public override void EnterState()
    {
        base.EnterState();

        skillManager.swordThrow.EnableDots(true);

        if (mainCamera != Camera.main)
            mainCamera = Camera.main;
    }

    public override void Update()
    {
        base.Update();

        Vector2 dirToMouse = DirectionToMouse();
        player.HandleFlip(dirToMouse.x);
        skillManager.swordThrow.PredictTrajectory(dirToMouse);

        player.SetVelocity(0, rb.linearVelocityY);

        if (player.InputAction.Player.Attack.WasPerformedThisFrame())
        {
            skillManager.swordThrow.ConfirmTrajectory(dirToMouse);
            skillManager.swordThrow.EnableDots(false);
            anim.SetBool("SwordthrowPerformed", true);
        }
        if (player.InputAction.Player.SwordThrow.WasReleasedThisFrame() || triggerCalled)
        {
            Debug.Log("Idle");
            fsm.ChangeState(player.idleState);
        }
    }
    public override void ExitState()
    {
        base.ExitState();
        anim.SetBool("SwordthrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 wordlMousePosition = mainCamera.ScreenToWorldPoint(player.mousePositionVector);

        Vector2 direction = wordlMousePosition - playerPosition;
        return direction.normalized;
    }
}
