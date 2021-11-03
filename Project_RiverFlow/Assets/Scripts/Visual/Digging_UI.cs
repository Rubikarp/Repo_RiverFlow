using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Digging_UI : MonoBehaviour
{
    public TextMeshProUGUI textGUI;

    public DigingHandler dig;

    void Update()
    {
        textGUI.text = dig.shovelHit.ToString() + " Coup de pelle";
    }
}
