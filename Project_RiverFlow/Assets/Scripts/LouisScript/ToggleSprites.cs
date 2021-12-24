using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class ToggleSprites : MonoBehaviour
{
    [BoxGroup("Statut")] public bool on;
    [Space(5)]
    [BoxGroup("Visual")] public Sprite sprOn;
    [BoxGroup("Visual")] public Sprite sprOff;
    private bool Sprite => !UI;
    [BoxGroup("Target")] public bool UI;
    [BoxGroup("Target")] [ShowIf("UI")] public Image image;
    [BoxGroup("Target")] [ShowIf("Sprite")] public SpriteRenderer spRenderer;

    public void ToggleGraph()
    {
        if(UI == true)
        {
            if (on == true)
            {
                image.sprite = sprOn;

            }
            else
            {
                image.sprite = sprOff;

            }
        }
        else
        {
            if (on == true)
            {
                spRenderer.sprite =sprOn;

            }
            else
            {
                spRenderer.sprite = sprOff;
            }
        }
        on = !on;
    }
}
