using UnityEngine;

public class AutoDestroyer : MonoBehaviour
{
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


    private void Start()
    {
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
        float range = Random.Range(minRotation,maxRotation);
        transform.Rotate(0, 0, range);
    }
}
