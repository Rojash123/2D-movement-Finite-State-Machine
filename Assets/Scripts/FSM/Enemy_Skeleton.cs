using UnityEngine;

public class Enemy_Skeleton : Enemy
{
    protected override void Awake()
    {
        base.Awake();
        idleState = new(fsm, "idle", this);
        moveState = new(fsm, "move", this);
        attackState = new(fsm, "attack", this);
        battleState = new(fsm, "battle", this);
        deadState = new(fsm, "idle", this);
        stunnedState = new(fsm, "stunned", this);
    }

    protected override void Start()
    {
        base.Start();
        fsm.InititalizeMethod(idleState);
    }
}
