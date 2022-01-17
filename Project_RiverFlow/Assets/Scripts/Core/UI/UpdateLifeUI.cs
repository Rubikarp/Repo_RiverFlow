using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLifeUI : MonoBehaviour
{
    public ScoreManager scoreManager;
    public Sprite pvAlive;
    public Sprite pvDead;
    public Image pv1;
    public Image pv2;
    public Image pv3;
    // Start is called before the first frame update
    void Start()
    {
        scoreManager = ScoreManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (scoreManager.exposedDeadPlants == 0)
        {
            pv1.sprite = pvAlive;
            pv2.sprite = pvAlive;
            pv3.sprite = pvAlive;
        }
        else if (scoreManager.exposedDeadPlants == 1)
        {
            pv1.sprite = pvDead;
            pv2.sprite = pvAlive;
            pv3.sprite = pvAlive;
        }
        else if (scoreManager.exposedDeadPlants == 2)
        {
            pv1.sprite = pvDead;
            pv2.sprite = pvDead;
            pv3.sprite = pvAlive;
        }
        else if (scoreManager.exposedDeadPlants >= 3)
        {
            pv1.sprite = pvDead;
            pv2.sprite = pvDead;
            pv3.sprite = pvDead;
        }
    }
}
