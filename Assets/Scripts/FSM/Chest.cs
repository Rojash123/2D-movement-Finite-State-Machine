using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private Animator animator=>GetComponentInChildren<Animator>();
    private Rigidbody2D rb => GetComponent<Rigidbody2D>();
    private Entity_VFX entityVFX => GetComponentInChildren<Entity_VFX>();

    [Header("Open Chest")]
    [SerializeField] private Vector2 knockBack;

    public void TakeDamage(float damage, Transform damageDealer)
    {
        entityVFX?.PlayOnDamageVFX();
        rb.linearVelocity = knockBack;
        rb.angularVelocity = Random.Range(-200f, 200f);
        animator.SetBool("chestOpen", true);
    }
}
