using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class Plant_Drawer : MonoBehaviour
{
    [Header("Réferences")]
    public Plant plant;

    [Header("Visual")]
    private SpriteRenderer sprRender;
    public PlantSprites_SCO visual;

    [Header("Living")]
    [SerializeField] Camera cam;
    [SerializeField] Canvas canvas;
    [SerializeField] Image imgTiller;

    void Start()
    {
        cam = Camera.main;
        canvas.worldCamera = cam;

        sprRender = gameObject.GetComponent<SpriteRenderer>();
        UpdateSkin();
        plant.onStateChange.AddListener(UpdateSkin);
    }

    void Update()
    {
        imgTiller.fillAmount = plant.timer;
    }

    private void UpdateSkin()
    {
        sprRender.sprite = visual.GetSprite(plant.currentState, plant.tileOn.type);
    }
}
