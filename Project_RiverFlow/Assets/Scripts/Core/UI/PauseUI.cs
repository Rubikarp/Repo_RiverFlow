using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseUI : MonoBehaviour
{
    private bool isPaused = false;
    //private GameObject pauseUI;
    private TimeManager timeManager;
    public RectTransform optionsPanel;
    public RectTransform pausePanel;
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

            if (isPaused)
            {
                pausePanel.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
                pausePanel.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);
                timeManager.isPaused = true;
            }
            else
            {
                timeManager.isPaused = false;
                optionsPanel.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
                optionsPanel.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
                pausePanel.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
                pausePanel.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);


            }
        }
    }

    public void Resume()
    {
        timeManager.isPaused = false;
        pausePanel.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        pausePanel.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
        isPaused = false;
        canvas.SetActive(false);



    }

    public void Options()
    {
        // Ouvrir l'UI des Options
        optionsPanel.DOScaleY(1f, 0.2f).SetEase(Ease.InOutBack);
        optionsPanel.DOScaleX(1f, 0.2f).SetEase(Ease.InOutBack);
    }

    public void ResumeOptions()
    {
        optionsPanel.DOScaleY(0f, 0.2f).SetEase(Ease.InOutBack);
        optionsPanel.DOScaleX(0f, 0.2f).SetEase(Ease.InOutBack);
        // fermer l'UI des Options

    }

    public void MainMenu()
    {

        timeManager.isPaused = false;
        ScoreManager.Instance.ReturnToMenu();
    }

}
