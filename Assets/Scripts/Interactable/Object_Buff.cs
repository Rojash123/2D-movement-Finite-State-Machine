using System;
using System.Collections;
using UnityEngine;

public class Object_Buff : MonoBehaviour
{
    private Player_Stats playerStats;

    [Header("Buff")]
    [SerializeField] private BuffEffectData[] buffs;
    [SerializeField] private string buffName;
    [SerializeField] private float buffDuration = 4f;

    [Header("Floating Movement")]
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
        transform.position = startPos + new Vector3(0, yOffset);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerStats = collision.GetComponent<Player_Stats>();

        if (playerStats.CanApplyBuffOn(buffName))
        {
            playerStats.ApplyBuff(buffs,buffDuration,buffName);
            Destroy(gameObject);
        }
    }
}

