using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Entity_VFX : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float vfxDuration = 0.2f;

    private Material originalMaterial;

    private Coroutine vfxCoroutine;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }
    public void PlayOnDamageVFX()
    {
        if (vfxCoroutine != null)
            StopCoroutine(vfxCoroutine);

        vfxCoroutine = StartCoroutine(OnDamageVfx());
    }

    private IEnumerator OnDamageVfx()
    {
        spriteRenderer.material = onDamageMaterial;
        yield return new WaitForSeconds(vfxDuration);
        spriteRenderer.material = originalMaterial;
    }

}
