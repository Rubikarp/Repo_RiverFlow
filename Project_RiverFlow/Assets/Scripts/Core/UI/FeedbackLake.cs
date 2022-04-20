using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackLake : MonoBehaviour
{
    public GameGrid grid;
    public InputHandler input;
    private GameTile currentTile;
    private Lake currentLake;
    bool isplaying = false;
    // Start is called before the first frame update
    void Start()
    {
        input = InputHandler.Instance;
        grid = GameGrid.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currentTile = grid.GetTile(grid.PosToTile(input.GetHitPos()));

        if (currentTile.element is Lake)
        {
            currentLake = currentTile.element as Lake;
            if (isplaying == false)
            {
                currentLake.previsual.Play(true);
                isplaying = true;
            }

        }
        else
        {
            currentLake.previsual.Stop(true);
            isplaying = false;
        }
    }
}
