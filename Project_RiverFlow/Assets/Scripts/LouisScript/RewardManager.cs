using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class WeightedButton
{
    public GameObject UsedButton;
    public int weight;
}
public class RewardManager : MonoBehaviour
{
    public List<WeightedButton> usedButtons;
    public GameObject SourceButton;
    public int sourceTurn;
    //public GameObject RewardDig;
    //public GameObject RewardCloud;
    //public GameObject RewardSource;
    //public GameObject RewardLake;
    List<GameObject> rewardDisplays;
    public RewardDisplay rewardButton;
    public GameObject Newday;
    public RectTransform rewardSelectionPanelTransform;
    public InventoryManager Inventory;
    public GameTime Timer;

    void Start()
    {
        rewardDisplays = new List<GameObject>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    InitializeSelectionPanel();
        //}
    }

    public void EmptySelectionPanel()
    {
        while (rewardSelectionPanelTransform.childCount != 0)
        {
            Newday.SetActive(false);
            DestroyImmediate(rewardSelectionPanelTransform.GetChild(0).gameObject);
            rewardSelectionPanelTransform.sizeDelta = new Vector2(0, 0);
            rewardDisplays.Clear();
        }
    }
    public void InitializeSelectionPanel()
    {

        Newday.SetActive(true);
        EmptySelectionPanel();

        List<WeightedButton> Temporary = new List<WeightedButton>(usedButtons);


        //le selection panel
        while (rewardSelectionPanelTransform.childCount != 0)
        {
            DestroyImmediate(rewardSelectionPanelTransform.GetChild(0).gameObject);
        }
        //tirage 
        if (Timer.weekNumber % sourceTurn == 0)
        {

            int pull = Random.Range(0, Temporary.Count);
            GameObject newRewardDisplay = Instantiate(Temporary[pull].UsedButton, rewardSelectionPanelTransform);
            rewardDisplays.Add(newRewardDisplay);
            Temporary.RemoveAt(pull);
            GameObject SourceRewardDisplay = Instantiate(SourceButton, rewardSelectionPanelTransform);
            rewardDisplays.Add(SourceRewardDisplay);
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject usedButton = RandomWeightedChoiceWithoutReplacement(Temporary);
                GameObject newRewardDisplay = Instantiate(usedButton, rewardSelectionPanelTransform);
                rewardDisplays.Add(newRewardDisplay);
            }
        }
        rewardSelectionPanelTransform.sizeDelta = new Vector2((rewardDisplays.Count - 1) * (rewardButton.GetComponent<RectTransform>().sizeDelta.x + rewardSelectionPanelTransform.GetComponent<HorizontalLayoutGroup>().spacing), rewardSelectionPanelTransform.sizeDelta.y);
    }


    private GameObject RandomWeightedChoiceWithoutReplacement(List<WeightedButton> weightedButtons)
    {
        List<int> weightIndexes = ComputeWeightIndexes(weightedButtons);
        int randomNumber = Random.Range(0, weightIndexes[weightIndexes.Count-1]);
        int indexToDrop = ComputeIndexOfButtonToDrop(weightIndexes, randomNumber);
        GameObject selectedButton = weightedButtons[indexToDrop].UsedButton;
        weightedButtons.Remove(weightedButtons[indexToDrop]);
        //Debug.Log(selectedButton);
        return selectedButton;
    }

    private static int ComputeIndexOfButtonToDrop(List<int> weightIndexes, int randomNumber)
    {
        int indexToDrop = 0;
        foreach (int index in weightIndexes)
        {
            if (randomNumber < index)
            {
                break;
            }
            indexToDrop++;
        }

        return indexToDrop;
    }

    private static List<int> ComputeWeightIndexes(List<WeightedButton> weithedButtons)
    {
        List<int> weightIndexes = new List<int>();
        int i = 0;
        foreach (WeightedButton weithedButton in weithedButtons)
        {
            weightIndexes.Add(i);
            i += weithedButton.weight;
        }
        weightIndexes.Add(i);
        return weightIndexes;
    }
}
