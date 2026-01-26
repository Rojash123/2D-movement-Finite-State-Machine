using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Entity_VFX : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    private Entity entity;

    [Header("On Damage VFX")]
    [SerializeField] private Material onDamageMaterial;
    [SerializeField] private float vfxDuration = 0.2f;

    private Material originalMaterial;
    private Coroutine vfxCoroutine;

    [Header("On DoingCDamage VFX")]
    [SerializeField] private GameObject vfxHit;
    [SerializeField] private GameObject critvfxHit;
    [SerializeField] private Color vfxColor = Color.white;

    [Header("ElementalAttack")]
    [SerializeField] private Color chillColor = Color.cyan;
    [SerializeField] private Color burnColor = Color.red;
    [SerializeField] private Color lightningColor = Color.yellow;
    private Color originalVFXColor;


    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
        entity = GetComponent<Entity>();
        originalVFXColor = vfxColor;
    }
    public void CreateHitVFX(Transform spawnPos, bool isCrit,ElementType element)
    {
        GameObject prefab = isCrit ? critvfxHit : vfxHit;
        var vfxObj = Instantiate(prefab, spawnPos.position, Quaternion.identity);

        if (!isCrit)
            vfxObj.GetComponentInChildren<SpriteRenderer>().color = GetElementColor(element);

        if (entity.facingDir == -1 && isCrit)
            vfxObj.transform.Rotate(0, 180, 0);
    }
    public Color GetElementColor(ElementType element)
    {
        switch (element)
        {
            case ElementType.Ice:
                return chillColor;
            case ElementType.Fire: 
                return burnColor;
            default:
                return Color.white;
        }
        ;
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
    public void PlayOnStatusVFX(float duration, ElementType element)
    {
        if (element == ElementType.Ice)
            StartCoroutine(PlayStatusVfxCo(duration, chillColor));

        if (element == ElementType.Fire)
            StartCoroutine(PlayStatusVfxCo(duration, burnColor));

        if (element == ElementType.Lightning)
            StartCoroutine(PlayStatusVfxCo(duration, lightningColor));
    }
    public void StopAllVFX()
    {
        StopAllCoroutines();
        spriteRenderer.color = Color.white;
        spriteRenderer.material = originalMaterial;
    }
    private IEnumerator PlayStatusVfxCo(float duration, Color colorEffect)
    {
        float tickInterval = 0.25f;
        float timer = 0;

        Color lightColor = colorEffect * 1.2f;
        Color darkColor = colorEffect * 0.8f;

        bool toogle = false;

        while (timer < duration)
        {
            spriteRenderer.color = toogle ? lightColor : darkColor;
            toogle = !toogle;
            yield return new WaitForSeconds(tickInterval);
            timer += tickInterval;
        }
        spriteRenderer.color = Color.white;
    }

}
