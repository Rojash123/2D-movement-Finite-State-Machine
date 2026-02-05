using UnityEngine;

[CreateAssetMenu(fileName = "ItemEffect-IceBlast", menuName = "Scriptable Objects/ItemEffect-IceBlast")]
public class ItemEffect_Blast : ItemEffectDataSO
{
    [SerializeField] private ElementalEffectData effectData;
    [SerializeField] private float iceDamage;
    [SerializeField] private LayerMask whatIsEnemy;

    [SerializeField] private float healtPercentageTrigger = 0.25f;
    [SerializeField] private float coolDown;
    private float lastTimeUsed = -999f;

    [SerializeField] private GameObject iceBlastVfx;
    [SerializeField] private GameObject OnHitVfx;

    public override void ExecuteEffect()
    {
        bool noCooldown = Time.time > lastTimeUsed + coolDown;
        bool reachHealthThreshhold = player.playerHealth.GetHealthPercentage() <= healtPercentageTrigger;

        if (noCooldown && reachHealthThreshhold)
        {
            player.vfx.CreateEffectOf(iceBlastVfx, player.transform);
            lastTimeUsed = Time.time;
            DamageEnemyWithIce();
        }
    }
    private void DamageEnemyWithIce()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 1.5f, whatIsEnemy);

        foreach (Collider2D collider in colliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();

            if (damageable == null) continue;

            bool targetGotHit = damageable.TakeDamage(0, iceDamage, ElementType.Ice, player.transform);
            Entity_StatusHandler entityStatus = collider.GetComponent<Entity_StatusHandler>();
            entityStatus?.ApplyEffect(ElementType.Ice, effectData);

            if (targetGotHit)
                player.vfx.CreateEffectOf(OnHitVfx, collider.transform);
        }
    }

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.playerHealth.OnDamageTaken += ExecuteEffect;
    }
    public override void UnSubscribe()
    {
        base.UnSubscribe();
        player.playerHealth.OnDamageTaken -= ExecuteEffect;
        player = null;
    }
}
