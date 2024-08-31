using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Typewriter : MonoBehaviour
{
    [SerializeField] private float speedMultiplier = 50f;
    public bool isSpeaking { get; private set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Coroutine coroutine;

    public void Run(string text, TMP_Text textLabel)
    {
        coroutine = StartCoroutine(TypeText(text, textLabel));
    }

    public void Stop()
    {
        StopCoroutine(coroutine);
        isSpeaking = false;
    }

    private IEnumerator TypeText(string text, TMP_Text textLabel)
    {
        isSpeaking = true;
        textLabel.text = string.Empty;
        yield return new WaitForSeconds(1);
        float t = 0;
        int i = 0;
        while(i < text.Length)
        {
            t += Time.deltaTime * speedMultiplier;
            i = Mathf.FloorToInt(t);
            i = Mathf.Clamp(i, 0, text.Length);
            textLabel.text = text.Substring(0, i);
            yield return null;
        }
        isSpeaking = false;
    }

    public void SkipText(string text, TMP_Text textLabel)
    {
        StopAllCoroutines();
        textLabel.text = text;
    }
}
