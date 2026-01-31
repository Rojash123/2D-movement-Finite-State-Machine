using UnityEngine;

public class Enemy_Health : Entity_Health
{
    private Enemy enemy => GetComponent<Enemy>();
    public override bool TakeDamage(float damage,float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (!canTakeDamage) return false;

        var takenDamage = base.TakeDamage(damage,elementalDamage,element, damageDealer);

        if (!takenDamage) return false;

        if (damageDealer.GetComponent<Player>() != null)
            enemy.TryEnterBattleState(damageDealer);

        return true;
    }
}
