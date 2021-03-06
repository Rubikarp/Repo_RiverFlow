using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ScoreManager : Singleton<ScoreManager>
{
    public TextMeshProUGUI scoreUI; 

    public GameObject gameOverText;
    private bool gameOn = true;
    private int deadPlants = 0;
    public int exposedDeadPlants = 0;
    public int gameScore = 0;
    private int moreScore = 0;
    public TimeManager gameTime;
    public ElementHandler elementHandler;
    //public int youngTreeScoring;
    //public int adultTreeScoring;
    //public int seniorTreeScoring;
    public int magicTreeScoring;
    public int lakeFishesScoring;
    private GameManager gameManager;
    public LevelSO level;
    public string menuPath;

    // Start is called before the first frame update
    void Start()
    {
        gameTime = TimeManager.Instance;
        gameManager = GameManager.Instance;

        gameOverText.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (!gameTime.isPaused)
        {
            VerifyDefeat();

            if (gameOn == false)
            {
                StartCoroutine(EndGame());
            }
        }
        scoreUI.text = gameScore.ToString();
    }

    private void UpdateScore()
    {
        //Debug.Log("Update");
        //for (int x = 0; x < elementHandler.allPlants.Count; x++)
        //{
        //    if (elementHandler.allPlants[x].currentState == PlantState.Young)
        //    {
        //        moreScore += youngTreeScoring;
        //    }
        //    else if (elementHandler.allPlants[x].currentState == PlantState.Adult)
        //    {
        //        moreScore += adultTreeScoring;
        //    }
        //    else if (elementHandler.allPlants[x].currentState == PlantState.Senior)
        //    {
        //        moreScore += seniorTreeScoring;
        //    }
        //}

        //for (int y = 0; y < elementHandler.allMagicTrees.Count; y++)
        //{
        //    if (elementHandler.allMagicTrees.Count != 0)
        //    {
        //        moreScore += magicTreeScoring;
        //    }
        //    Debug.Log("MagicTree");
        //}

        //for (int w = 0; w < elementHandler.allLakes.Count; w++)
        //{
        //    if (elementHandler.allLakes[w].hasFish == true)
        //    {
        //        moreScore += lakeFishesScoring;
        //    }
        //}

        //gameScore += moreScore;
        //moreScore = 0;
        //scoreUI.text = gameScore.ToString();
    }

    private void VerifyDefeat()
    {
        for (int y = 0; y < elementHandler.allPlants.Count; y++)
        {
            if (elementHandler.allPlants[y].currentState == PlantState.Dead)
            {
                deadPlants++;
            }
        }

        if (deadPlants < 3)
        {
            gameOn = true;
            exposedDeadPlants = deadPlants;
        }
        else if (deadPlants >= 3)
        {
            gameOn = false;
            exposedDeadPlants = deadPlants;
        }

        deadPlants = 0;
    }

    private IEnumerator EndGame()
    {
        gameTime.isPaused = true;
        LevelSoundboard.Instance.PlayLoserTheme();
        yield return new WaitForSeconds(1);
        gameOverText.SetActive(true);
        gameOverText.transform.DOScaleY(1.5f, 1.5f).SetEase(Ease.OutElastic);
        gameOverText.transform.DOScaleX(1.5f, 1.5f).SetEase(Ease.OutElastic);

        yield return new WaitForSeconds(5);

        Debug.Log("Return to menu");

        ReturnToMenu();

    }

    public void ReturnToMenu()
    {
        if(gameManager != null)
        {
            gameManager.levelSaves[gameManager.levelList.IndexOf(level)].levelRecord = gameScore;
            gameManager.SaveLevels();
        }
        GameManager.ChangeScene(menuPath);
    }
}
