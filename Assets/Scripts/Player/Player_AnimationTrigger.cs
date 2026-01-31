using UnityEngine;

public class Player_AnimationTrigger : EntityAnimationTrigger
{
   private Player player;

    protected override void Awake()
    {
        base.Awake();
        player=GetComponentInParent<Player>();
    }

    public void ThrowSword()=>player.skillManager.swordThrow.ThrowSword();
}
