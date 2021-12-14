using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreUI;
    public GameObject gameOverText;
    private bool gameOn = true;
    private int deadPlants = 0;
    private int gameScore = 0;
    private int moreScore = 0;
    public GameTime gameTime;
    public float scoringTick;
    private float scoringTimer = 0.0f;
    public ElementHandler elementHandler;
    public int youngTreeScoring;
    public int adultTreeScoring;
    public int seniorTreeScoring;
    public int magicTreeScoring;
    public int lakeFishesScoring;
    private GameManager gameManager;
    public LevelSO level;
    public string menuPath;

    // Start is called before the first frame update
    void Start()
    {
        gameTime = GameTime.Instance;
        gameManager = GameManager.Instance;

        gameOverText.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.levelSaves[gameManager.levelList.IndexOf(level)].levelRecord = gameScore;
            gameManager.SaveLevels();
            gameManager.ChangeScene(menuPath);
        }
        if (!gameTime.isPaused)
        {
            VerifyDefeat();

            if (gameOn == true)
            {
                scoringTimer += Time.deltaTime * gameTime.gameTimeSpeed;

                if (scoringTimer >= scoringTick)
                {
                    UpdateScore();
                    scoringTimer = 0;
                }
            }
            else if (gameOn == false)
            {
                EndGame();
            }
        }
    }

    private void UpdateScore()
    {
        Debug.Log("Update");
        for (int x = 0; x < elementHandler.allPlants.Count; x++)
        {
            if (elementHandler.allPlants[x].currentState == PlantState.Young)
            {
                moreScore += youngTreeScoring;
            }
            else if (elementHandler.allPlants[x].currentState == PlantState.Adult)
            {
                moreScore += adultTreeScoring;
            }
            else if (elementHandler.allPlants[x].currentState == PlantState.Senior)
            {
                moreScore += seniorTreeScoring;
            }
        }

        for (int y = 0; y < elementHandler.allMagicTrees.Count; y++)
        {
            if (elementHandler.allMagicTrees.Count != 0)
            {
                moreScore += magicTreeScoring;
            }
            Debug.Log("MagicTree");
        }

        for (int w = 0; w < elementHandler.allLakes.Count; w++)
        {
            if (elementHandler.allLakes[w].hasFish == true)
            {
                moreScore += lakeFishesScoring;
            }
        }

        gameScore += moreScore;
        moreScore = 0;
        scoreUI.text = gameScore.ToString();
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
        }
        else if (deadPlants >= 3)
        {
            gameOn = false;
        }
    }

    private void EndGame()
    {
        gameTime.isPaused = true;
        gameOverText.SetActive(true);

        gameManager.levelSaves[gameManager.levelList.IndexOf(level)].levelRecord = gameScore;
        gameManager.SaveLevels();
        gameManager.ChangeScene(menuPath);
    }
}
