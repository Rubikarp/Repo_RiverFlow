using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITimeSize : MonoBehaviour
{
    public GameObject pause;
    public GameObject play;
    public GameObject speed2;
    public GameObject speed3;
    private GameTime gameTime;
    public float sizeChange = 1.2f;

    //public Color pressedColor;

    void Awake()
    {
        gameTime = GameTime.Instance;
    }


    void Update()
    {
        if (gameTime.isPaused == true)
        {
            pause.transform.localScale = Vector3.one * sizeChange;
            play.transform.localScale = Vector3.one * 1;
            speed2.transform.localScale = Vector3.one * 1;
            speed3.transform.localScale = Vector3.one * 1;
        }
        else if(gameTime.gameTimeSpeed == 1)
        {
            pause.transform.localScale = Vector3.one * 1;
            play.transform.localScale = Vector3.one * sizeChange;
            speed2.transform.localScale = Vector3.one * 1;
            speed3.transform.localScale = Vector3.one * 1;
        }
        else if (gameTime.gameTimeSpeed == 2)
        {
            pause.transform.localScale = Vector3.one * 1;
            play.transform.localScale = Vector3.one * 1;
            speed2.transform.localScale = Vector3.one * sizeChange;
            speed3.transform.localScale = Vector3.one * 1;
        }
        else if (gameTime.gameTimeSpeed == 3)
        {
            pause.transform.localScale = Vector3.one * 1;
            play.transform.localScale = Vector3.one * 1;
            speed2.transform.localScale = Vector3.one * 1;
            speed3.transform.localScale = Vector3.one * sizeChange;
        }
        else
        {
            pause.transform.localScale = Vector3.one * 1;
            play.transform.localScale = Vector3.one * 1;
            speed2.transform.localScale = Vector3.one * 1;
            speed3.transform.localScale = Vector3.one * 1;
        }

    }
}
