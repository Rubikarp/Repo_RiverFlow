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
    }
}
