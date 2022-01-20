using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    private bool isPaused = false;
    private GameObject pauseUI;
    private TimeManager timeManager;

    private void Start()
    {
        pauseUI = GameObject.Find("PauseUI");
        timeManager = GameObject.Find("Timer").GetComponent<TimeManager>();
        pauseUI.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            isPaused = !isPaused;
            pauseUI.SetActive(!isPaused);
            
            if(isPaused)
            {
                Time.timeScale = 0;
                timeManager.gameTimeSpeed = 0;
            }
            else
            {
                Time.timeScale = 1;
                timeManager.gameTimeSpeed = 1;
            }
        }
    }

    public void Resume()
    {
        isPaused = false;
        pauseUI.SetActive(false);

        Time.timeScale = 1;
        timeManager.gameTimeSpeed = 1;
    }

    public void Options()
    {
        // Ouvrir l'UI des Options
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("_MainMenu");
    }

}
