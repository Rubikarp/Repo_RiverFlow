using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelDisplay : MonoBehaviour
{
    public LevelSO level;

    public Text nameText;
    //public Text descriptionText;

    public Image scoreImage;
    public Text levelNumberText;
    public int levelNumber;
    private Button self;
    void Start()
    {
        UpdateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateDisplay()
    {
        nameText.text = level.name;
        //descriptionText.text = level.description;
        levelNumber = level.levelNumber;
        levelNumberText.text = "Level " + levelNumber.ToString();
        self = 
    }

    public void GoToScene()
    {
        GameManager.Instance.ChangeScene(level.levelSceneName);
    }
    public void Unlock()
    {
        if(level.scoreUnlock <= level.LockLvl.recordScore)
        {
            
        }
    }
}
