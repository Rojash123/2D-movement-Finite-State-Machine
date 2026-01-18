using System;
using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Entity_Stats entity_Stats;

    [Header("Buff")]
    [SerializeField] private Buff[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4f;
    [SerializeField] private bool canBeUsed = true;


    [Header("Floating Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPos + new Vector3(0, yOffset);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canBeUsed) return;
        entity_Stats = collision.GetComponent<Entity_Stats>();
        StartCoroutine(BuffTimer(buffDuration));
    }

    private IEnumerator BuffTimer(float duration)
    {
        canBeUsed = false;
        spriteRenderer.color = Color.clear;
        foreach (var item in buffs)
        {
            Stat stat = entity_Stats.GetStat(item.type);
            stat.AddModifier(item.buffValue, buffName);
        }
        yield return new WaitForSeconds(duration);
        foreach (var item in buffs)
        {
            Stat stat = entity_Stats.GetStat(item.type);
            stat.RemoveModifier(buffName);
        }
        Destroy(gameObject);
    }

}
[Serializable]
public class Buff
{
    public StatType type;
    public float buffValue;
}
