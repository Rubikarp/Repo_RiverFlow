using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poc_PlantSpawnerScript : MonoBehaviour
{
    public GameObject[] plants;
    private float timer = 0.0f;
    public float plantSpawnTimer;
    private int plantIndex = 0;
    private bool allSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < plants.Length; i++)
        {
            plants[i].SetActive(false);
        }

        plants[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (allSpawned == false)
        {
            timer += Time.deltaTime;

            if (timer >= plantSpawnTimer)
            {
                SpawnNextPlant();
                timer = 0.0f;
            }
        }
    }

    private void SpawnNextPlant()
    {
        plantIndex += 1;
        plants[plantIndex].SetActive(true);

        if (plantIndex == plants.Length - 1)
        {
            allSpawned = true;
        }
    }
}
