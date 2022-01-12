using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeSounds : MonoBehaviour
{
    public string timeButtonSound = "TimeButton";
    public void UITimeSoundPlay()
    {
        LevelSoundboard.Instance.PlayTimeUISound(timeButtonSound);
    }

}
