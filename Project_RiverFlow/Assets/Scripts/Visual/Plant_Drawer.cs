using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plant_Drawer : MonoBehaviour
{
    [Header("Réferences")]
    public Plant plant;

    [Header("Visual")]
    public GameObject[] allSkins;

    [Header("Living")]
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;
    [SerializeField] Image imgTiller;

    void Start()
    {
        cam = Camera.main;
        canvas.worldCamera = cam;

        UpdateSkin();
        plant.onStateChange.AddListener(UpdateSkin);
    }

    void Update()
    {
        imgTiller.fillAmount = plant.timer;
    }

    private void UpdateSkin()
    {
        for (int i = 0; i < allSkins.Length; i++)
        {
            allSkins[i].SetActive(false);
        }

        allSkins[(int)plant.currentState].SetActive(true);
    }
}
