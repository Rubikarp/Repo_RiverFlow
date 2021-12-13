using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardManager : MonoBehaviour
{
    public List<GameObject> usedButtons;
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
            rewardSelectionPanelTransform.sizeDelta = new Vector2(0,0);
        }
    }
    public void InitializeSelectionPanel()
    {
        Newday.SetActive(true);
        EmptySelectionPanel();
        // faire le random
        List<GameObject> Temporary = new List<GameObject>();
           if (usedButtons.Count > 0)
           {

                for (int i = 0; i < usedButtons.Count; i++)
                {
                Temporary.Add(usedButtons[i]);
                }
           }

    //le selection panel
        while (rewardSelectionPanelTransform.childCount != 0)
        {
            DestroyImmediate(rewardSelectionPanelTransform.GetChild(0).gameObject);
        }
        //tirage 
        if (Timer.weekNumber % sourceTurn == 0)
        {

                int pull = Random.Range(0, Temporary.Count);
                GameObject newRewardDisplay = Instantiate(Temporary[pull], rewardSelectionPanelTransform);
                rewardDisplays.Add(newRewardDisplay);
                Temporary.RemoveAt(pull);
                GameObject SourceRewardDisplay = Instantiate(SourceButton, rewardSelectionPanelTransform);
                rewardDisplays.Add(SourceRewardDisplay);
           


        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                int pull = Random.Range(0, Temporary.Count);
                GameObject newRewardDisplay = Instantiate(Temporary[pull], rewardSelectionPanelTransform);
                rewardDisplays.Add(newRewardDisplay);
                Temporary.RemoveAt(pull);
            }
        }
        
        rewardSelectionPanelTransform.sizeDelta = new Vector2((rewardDisplays.Count - 1) * (rewardButton.GetComponent<RectTransform>().sizeDelta.x + rewardSelectionPanelTransform.GetComponent<HorizontalLayoutGroup>().spacing), rewardSelectionPanelTransform.sizeDelta.y);


    }

}
