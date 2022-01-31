using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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
    public GameObject clock;
    public Color newDayColor;
    public Color newDayColorFade;
    public RectTransform rewardSelectionPanelTransform;
    public InventoryManager Inventory;
    public TimeManager Timer;

    public string rewardTimeSound = "RewardTime";

    public int[] basicRewardValue;
    public int[] weekMilestones;

    void Start()
    {
        Inventory = InventoryManager.Instance;
        rewardDisplays = new List<GameObject>();
        Timer = TimeManager.Instance;
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
            clock.transform.localScale = new Vector3(0, 0, 0);
            Newday.GetComponent<RawImage>().color = newDayColorFade;
            DestroyImmediate(rewardSelectionPanelTransform.GetChild(0).gameObject);
            rewardSelectionPanelTransform.sizeDelta = new Vector2(0, 0);
            rewardDisplays.Clear();
        }
    }
    public void InitializeSelectionPanel()
    {
        EmptySelectionPanel();
        Newday.SetActive(true);
        LevelSoundboard.Instance.PlayRewardUISound(rewardTimeSound);


        Newday.GetComponent<RawImage>().DOColor(newDayColor, 0.5f);
        clock.transform.DOScaleY(1f, 0.1f).SetEase(Ease.OutBack);
        clock.transform.DOScaleX(1f, 0.1f).SetEase(Ease.OutBack);
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
            AttributeDigRewards(newRewardDisplay, false);
            Temporary.RemoveAt(pull);
            GameObject SourceRewardDisplay = Instantiate(SourceButton, rewardSelectionPanelTransform);
            rewardDisplays.Add(SourceRewardDisplay);
            AttributeDigRewards(SourceRewardDisplay, true);
        }
        else
        {
            if (Temporary.Any())
            {
                //Debug.Log("Temp not empty" + Temporary.Any());
                for (int i = 0; i < 2; i++)
                {
                    //Debug.Log("Search for reward n°" + i);
                    //foreach (var item in Temporary)
                    //{
                    //    Debug.Log(item.UsedButton.name);
                    //    Debug.Log(item.weight);

                    //}
                    GameObject usedButton = RandomWeightedChoiceWithoutReplacement(Temporary);
                    //Debug.Log("Button to Display : " + usedButton.name);
                    GameObject newRewardDisplay = Instantiate(usedButton, rewardSelectionPanelTransform);
                    rewardDisplays.Add(newRewardDisplay);
                    AttributeDigRewards(newRewardDisplay, false);
                }
            }
        }
        rewardSelectionPanelTransform.sizeDelta = new Vector2((rewardDisplays.Count - 1) * (rewardButton.GetComponent<RectTransform>().sizeDelta.x + rewardSelectionPanelTransform.GetComponent<HorizontalLayoutGroup>().spacing), rewardSelectionPanelTransform.sizeDelta.y);
    }


    private GameObject RandomWeightedChoiceWithoutReplacement(List<WeightedButton> weightedButtons)
    {
        List<int> weightIndexes = ComputeWeightIndexes(weightedButtons);
        //foreach (var item in weightIndexes)
        //{
        //    Debug.Log("Weight indexes : " + item);
        //}
        //Debug.Log("weightIndexes count - 1: " + (weightIndexes.Count - 1));
        int randomNumber = Random.Range(0, weightIndexes[weightIndexes.Count - 1]);
        //Debug.Log("randomNumber : " + randomNumber);
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
            i += weithedButton.weight;
            weightIndexes.Add(i);
        }
        return weightIndexes;
    }

    private void AttributeDigRewards(GameObject reward, bool isSource)
    {
        if (Timer.weekNumber < weekMilestones[0])
        {
            reward.GetComponent<RewardDisplay>().digBonus = basicRewardValue[0];
        }
        else if (Timer.weekNumber < weekMilestones[1])
        {
            reward.GetComponent<RewardDisplay>().digBonus = basicRewardValue[1];
        }
        else if (Timer.weekNumber < weekMilestones[2])
        {
            reward.GetComponent<RewardDisplay>().digBonus = basicRewardValue[2];
        }
        else if (Timer.weekNumber < weekMilestones[3])
        {
            reward.GetComponent<RewardDisplay>().digBonus = basicRewardValue[3];
        }
        else if (Timer.weekNumber < weekMilestones[4])
        {
            reward.GetComponent<RewardDisplay>().digBonus = basicRewardValue[4];
        }
        else if (Timer.weekNumber >= weekMilestones[4])
        {
            reward.GetComponent<RewardDisplay>().digBonus = basicRewardValue[5];
        }

        if (isSource == true)
        {
            reward.GetComponent<RewardDisplay>().digBonus -= 5;
        }
        if (reward.GetComponent<RewardDisplay>().cloudBonus == 0 && reward.GetComponent<RewardDisplay>().sourceBonus == 0 && reward.GetComponent<RewardDisplay>().lakeBonus == 0 && reward.GetComponent<RewardDisplay>().tunnelBonus == 0)
        {
            reward.GetComponent<RewardDisplay>().digBonus += 5;
        }
    }
}
