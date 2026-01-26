using System.Collections;
using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
    private SpriteRenderer sr;

    public bool isAutoDestroy = true;
    public float destroyDelay = 1f;

    public bool randomPosition = true;
    public bool randomRotation = true;

    [Header("Rotation Offset")]
    [SerializeField] private float minRotation = 0;
    [SerializeField] private float maxRotation = 360;


    [Header("Random Offset")]
    [SerializeField] private float xMinOffset = -0.3f;
    [SerializeField] private float xMaxOffset = 0.3f;
    [SerializeField] private float yMinOffset = -0.3f;
    [SerializeField] private float yMaxOffset = 0.3f;

    [Header("Fade Effect")]
    [SerializeField] private bool canFade;
    [SerializeField] private float fadeSpeed = 1;

    IEnumerator FadeCO()
    {
        Color targetColor = Color.white;
        while (targetColor.a > 0)
        {
            targetColor.a -= fadeSpeed * Time.deltaTime;
            sr.color = targetColor;
            yield return null;
        }
        sr.color = targetColor;
    }


    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        if (canFade)
            StartCoroutine(FadeCO());

        ApplyRandomOffset();
        ApplyRandomRotation();

        if (isAutoDestroy)
            Destroy(gameObject, destroyDelay);
    }
    void ApplyRandomOffset()
    {
        if (!randomPosition) return;

        float xOffset = Random.Range(xMinOffset, xMaxOffset);
        float yOffset = Random.Range(yMinOffset, yMaxOffset);

        transform.position += new Vector3(xOffset, yOffset);
    }
    void ApplyRandomRotation()
    {
        float range = Random.Range(minRotation, maxRotation);
        transform.Rotate(0, 0, range);
    }
}
