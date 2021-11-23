using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    public GameTile tileOn;
    private GameTime gameTime;

    // Start is called before the first frame update
    void Awake()
    {
        gameTime = GameTime.Instance;
        gameTime.onWaterSimulationStep.AddListener(FlowStep);
    }

    // Update is called once per frame
    void FlowStep()
    {
        if (tileOn.linkedTile.Count >= 2)
        {
            tileOn.receivedFlow += 1;
        }
    }
}
