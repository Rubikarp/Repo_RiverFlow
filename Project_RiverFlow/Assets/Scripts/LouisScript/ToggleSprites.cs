using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSprites : MonoBehaviour
{
    public Sprite sprOn;
    public Sprite sprOff;
    public bool UI;
    public Image image;
    public SpriteRenderer spRenderer;
    public bool on;

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
