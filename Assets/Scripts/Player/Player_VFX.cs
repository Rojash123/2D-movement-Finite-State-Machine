using System;
using System.Collections;
using UnityEngine;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")]
    [Range(0.01f, 0.2f)]
    [SerializeField] private float imageEchoInterval = 0.05f;
    [SerializeField] private GameObject imageEchoPrefabVFX;

    private Coroutine imageEchoCO;
    public void DoImageEffect(float duration)
    {
        if(imageEchoCO!=null)
            StopCoroutine(imageEchoCO);

        imageEchoCO = StartCoroutine(ImageEchoEffectCO(duration));
    }

    private IEnumerator ImageEchoEffectCO(float duration)
    {
        float time = 0;
        while (time < duration)
        {
            CreateImageEcho();
            yield return new WaitForSeconds(imageEchoInterval);
            time += imageEchoInterval;
        }
    }
    private void CreateImageEcho()
    {
        GameObject echo = Instantiate(imageEchoPrefabVFX, transform.position, transform.rotation);
        echo.GetComponentInChildren<SpriteRenderer>().sprite = spriteRenderer.sprite;
    }

}
