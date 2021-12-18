using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGrid_Drawer : MonoBehaviour
{
    [Header("reference")]
    public GameGrid grid;
    public InputHandler input;
    public SpriteRenderer rend;

    [Header("Parameter")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float colorSpeed = 5f;
    [Space(10)]
    [SerializeField, Range(0, 1.00f)] private float opacity = 0.66f;
    [SerializeField, Range(0, 2.00f)] private float roundness = 1f;
    [SerializeField, Range(0, 0.25f)] private float thickness = 0.1f;
    [Space(10)]
    [SerializeField, ColorUsage(true, false)] private Color baseColor;
    [SerializeField, ColorUsage(true, false)] private Color pencilColor;
    [SerializeField, ColorUsage(true, false)] private Color eraserColor;
    
    [Header("Internal")]
    private MaterialPropertyBlock propBlock;
    [SerializeField] private float time = 0f;
    [SerializeField] private float colorTime = 0f;


    void Start()
    {
        transform.localScale = new Vector3(grid.size.x, grid.size.y, 1);

        //permet d'overide les param sans modif le mat ou créer d'instance
        propBlock = new MaterialPropertyBlock();
        //Recup Data
        rend.GetPropertyBlock(propBlock);

        //EditZone
        propBlock.SetFloat("_Alpha", 0);
        propBlock.SetFloat("_Thickness", 0);
        propBlock.SetFloat("_Roundness", roundness);
        propBlock.SetColor("_Color", pencilColor);

        //Push Data
        rend.SetPropertyBlock(propBlock);
    }

    void Update()
    {
        //OnDrag
        if (Input.GetMouseButton(0))
        {
            time += Time.deltaTime * speed;
            colorTime += Time.deltaTime * colorSpeed;

            time = Mathf.Clamp01(time);
            colorTime = Mathf.Clamp01(colorTime);

            rend.color = Color.Lerp(baseColor, pencilColor, Mathf.Pow(colorTime, 3));
        }
        else
        if(Input.GetMouseButton(1))
        {
            time += Time.deltaTime * speed;
            colorTime += Time.deltaTime * colorSpeed;

            time = Mathf.Clamp01(time);
            colorTime = Mathf.Clamp01(colorTime);

            rend.color = Color.Lerp(baseColor, eraserColor, Mathf.Pow(colorTime, 3));
        }
        else
        {
            time -= Time.deltaTime * speed;
            colorTime -= Time.deltaTime * colorSpeed;

            time = Mathf.Clamp01(time);
            colorTime = Mathf.Clamp01(colorTime);

            rend.color = Color.Lerp(baseColor, rend.color, Mathf.Pow(colorTime, 3));
        }

        //permet d'overide les param sans modif le mat ou créer d'instance
        propBlock = new MaterialPropertyBlock();
        //Recup Data
        rend.GetPropertyBlock(propBlock);

        //EditZone
        propBlock.SetFloat("_Alpha", Mathf.Lerp(0, opacity, Mathf.Clamp01(Mathf.Pow(time, 2))));
        propBlock.SetFloat("_Thickness", Mathf.Lerp(0, thickness, Mathf.Clamp01(Mathf.Pow(time, 2))));
        propBlock.SetFloat("_Roundness", roundness);

        //Push Data
        rend.SetPropertyBlock(propBlock);

    }
}
