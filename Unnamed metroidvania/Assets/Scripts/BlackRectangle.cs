using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackRectangle : MonoBehaviour
{
    public float fadeSpeed;
    public Image image;
    public Coroutine coroutine;

    private void Start()
    {
        Unfade();
    }

    public Coroutine Fade()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(FadeRoutine());
        return coroutine;
    }

    IEnumerator FadeRoutine()
    {
        while(image.color.a < 1)
        {
            Color color = image.color;
            color.a += fadeSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }
    }

    public Coroutine Unfade()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(UnfadeRoutine());
        return coroutine;
    }

    IEnumerator UnfadeRoutine()
    {
        while (image.color.a > 0)
        {
            Color color = image.color;
            color.a -= fadeSpeed * Time.deltaTime;
            image.color = color;
            yield return null;
        }
        
    }
}
