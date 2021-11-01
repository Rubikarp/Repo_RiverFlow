using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverManager : MonoBehaviour
{
    public InputHandler input;
    public DigingHandler digging;

    public List<Canal> canals = new List<Canal>();

    void Start()
    {
        /*
        input.onLeftClickUp.AddListener();
        input.onLeftClicking.AddListener();
        input.onLeftClickDown.AddListener();
        input.onRightClickUp.AddListener();
        input.onRightClicking.AddListener();
        input.onRightClickDown.AddListener();
        */
        digging.onLink.AddListener(OnLink);
    }

    void Update()
    {
        
    }

    public void OnLink(GameTile startTile, GameTile endTile)
    {
        Canal canal = new Canal(startTile, endTile, new List<GameTile>(), startTile.riverStrenght);

        //Check if isloate (no tile have more than 2 link)
        if (false)
        {
            //Check Prolonging an existing canal (end/start have than 2 or more link)
            if (false)
            {

            }
            else
            {
                //Check Prolonging an existing canal
                if (false)
                {

                }
                else
                {

                }

            }
        }
        else
        {
            canals.Add(canal);
        }
    }


}
