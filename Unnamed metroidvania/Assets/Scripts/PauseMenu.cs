using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public Canvas canvas;
    public static bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvas.enabled)
            {
                UnpauseGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        canvas.enabled = true;
        isOpen = true;
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1;
        canvas.enabled = false;
        isOpen = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}