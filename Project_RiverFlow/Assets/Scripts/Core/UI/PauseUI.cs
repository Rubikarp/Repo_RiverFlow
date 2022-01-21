using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseUI : MonoBehaviour
{
    private bool isPaused = false;
    //private GameObject pauseUI;
    private TimeManager timeManager;
    public GameObject options;
    public GameObject canvas;
    private void Start()
    {
        
        timeManager = TimeManager.Instance;
        canvas.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {

            isPaused = !isPaused;
            canvas.SetActive(isPaused);
            options.SetActive(false);
            if (isPaused)
            {
                Time.timeScale = 0;
                timeManager.SetSpeed(0);
            }
            else
            {
                Time.timeScale = 1;
                timeManager.SetSpeed(1);
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        canvas.SetActive(false);

        Time.timeScale = 1;
        timeManager.SetSpeed(1);
    }

    public void Options()
    {
        // Ouvrir l'UI des Options
        options.SetActive(true);
    }

    public void ResumeOptions()
    {
        // Ouvrir l'UI des Options
        options.SetActive(false);
    }

    public void MainMenu()
    {
        ScoreManager.Instance.ReturnToMenu();
    }

}
