using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Scriptable Objects/HealEffect")]
public class ItemEffect_Heal : ItemEffectDataSO
{
    [SerializeField] private float healPercent = 0.1f;
    public override void ExecuteEffect()
    {
        Player player=FindFirstObjectByType<Player>();

        float healAmount=player.entityStats.GetMaxHealth()* healPercent;
        player.playerHealth.IncreaseHealth(healAmount);
    }
}
