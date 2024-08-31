using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float slowmoFactor = 0.05f;
    public float slowmoDuration = 2f;
    //public float slowmoTimer;

    // Update is called once per frame
    void Update()
    {
        Time.timeScale += (1f / slowmoDuration) * Time.unscaledDeltaTime;
        Time.fixedDeltaTime += (0.01f / slowmoDuration) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0f, 0.01f);
    }

    public void Slowmo()
    {
        Time.timeScale = slowmoFactor;
        Time.fixedDeltaTime = Time.fixedDeltaTime * slowmoFactor;
    }
}
