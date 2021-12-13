using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayCountUI : MonoBehaviour
{
    private TextMeshProUGUI textGUI;
    public GameTime Timer;
    //public DigingHandler dig;
    private void Start()
    {
        textGUI = this.gameObject.GetComponent<TextMeshProUGUI>();
        
    }
    private void Update()
    {
        UpdateSelf();
    }
    public void UpdateSelf()
    {
        textGUI.text ="Day " + Timer.weekNumber.ToString();
    }
}
