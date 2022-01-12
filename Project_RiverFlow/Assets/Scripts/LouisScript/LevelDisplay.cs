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

    public Image levelImage;
    public GameObject levelImageObject;
    public Text levelRecordText;
    public int levelNumber;
    [SerializeField]
    private Button self;
    public Image levelbackgroundImage;
    public Sprite levelbackgroundLocked;
    public GameObject playButton;
    public GameObject textScore;
    public GameObject unlockTextObject;
    public Text unlockText;


    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateDisplay(int record)
    {
        
        nameText.text = level.name;
        //descriptionText.text = level.description;
        levelNumber = level.levelNumber;
        levelRecordText.text = "High score " + record.ToString();
        Unlock(GameManager.Instance.levelList.IndexOf(level.LockLvl));
        levelImage.sprite = level.levelCapture;
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
            self.interactable = true;
        }
        else
        {
            levelbackgroundImage.sprite = levelbackgroundLocked;
            levelImageObject.SetActive(false);
            playButton.SetActive(false);
            textScore.SetActive(false);
            unlockTextObject.SetActive(true);
            unlockText.text = "You need " + level.scoreUnlock + " butterflies on the precedent level to unlock.";

        }
    }
}
