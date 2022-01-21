using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsManager : MonoBehaviour
{
    public List<LevelSO> currentLevels;

    public LevelDisplay levelButton;
    public RectTransform levelSelectionPanelTransform;
    List<LevelDisplay> levelDisplays;
    public Camera menuCamera;
    public Vector3 mainMenuCameraPos;
    public Vector3 levelSelectionCameraPos;
    public Vector3 optionMenuCameraPos;
    public Vector3 CreditMenuCameraPos;
    public GameObject cameraFollower;


    void Start()
    {
        levelDisplays = new List<LevelDisplay>();
    }

    public void InitializeSelectionPanel()
    {

        levelDisplays.Clear();
        KarpHelper.DeleteChildrens(levelSelectionPanelTransform);

        currentLevels = GameManager.Instance.levelList;
        for (int i = 0; i < currentLevels.Count; i++)
        {
            LevelDisplay newleveldisplay = Instantiate(levelButton, levelSelectionPanelTransform);
            newleveldisplay.level = currentLevels[i];
            newleveldisplay.UpdateDisplay(GameManager.Instance.levelSaves[i].levelRecord);

            levelDisplays.Add(newleveldisplay);
        }

        levelSelectionPanelTransform.sizeDelta = new Vector2(
            (currentLevels.Count - 1) * (levelButton.GetComponent<RectTransform>().sizeDelta.x + levelSelectionPanelTransform.GetComponent<HorizontalLayoutGroup>().spacing), 
            levelSelectionPanelTransform.sizeDelta.y);

        //moveToLevelSelection();
    }

    public void moveToLevelSelection()
    {
        //Debug.Log(cameraFollower);
        cameraFollower.transform.position = levelSelectionCameraPos;
    }

    public void moveToMainMenu()
    {
        //Debug.Log("test");
        cameraFollower.transform.position = mainMenuCameraPos;
    }

    public void moveToOptionMenu()
    {
        //Debug.Log("test");
        cameraFollower.transform.position = optionMenuCameraPos;
    }

    public void moveToCreditMenu()
    {
        //Debug.Log("test");
        cameraFollower.transform.position = CreditMenuCameraPos;
    }

}
