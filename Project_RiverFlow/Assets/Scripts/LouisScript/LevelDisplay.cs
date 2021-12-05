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
    public Text levelRecordText;
    public int levelNumber;
    [SerializeField]
    private Button self;


    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateDisplay(int record)
    {
        
        nameText.text = level.name;
        //descriptionText.text = level.description;
        levelNumber = level.levelNumber;
        levelNumberText.text = "Level " + levelNumber.ToString();
        levelRecordText.text = "Score " + record.ToString();
        Unlock(GameManager.Instance.levelList.IndexOf(level.LockLvl));
        
    }

    public void GoToScene()
    {
        GameManager.Instance.ChangeScene(level.levelSceneName);
    }
    public void Unlock(int lockerPos)
    {
        if(level.scoreUnlock <= GameManager.Instance.levelSaves[lockerPos].levelRecord)
        {
            //unlocked
            self.interactable =true;
        }
        else
        {
            self.interactable = false;
;
        }
    }
}
