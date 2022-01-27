using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireFrameView : MonoBehaviour
{
    [Header("reference")]
    public InputHandler input;
    [Space(10)]
    public Renderer gridRend;
    public Renderer bgRend;

    [Header("Settings")]
    [SerializeField, Range(0.05f, 1.0f)] private float durUp = 0.2f;
    [SerializeField, Range(0.05f, 1.0f)] private float durDown = 0.2f;

    [Header("Parameter")]
    [SerializeField, Range(0, 1.00f)] private float gridMaxOpacity = 0.3f;
    [SerializeField, Range(0, 0.25f)] private float gridMaxThickness = 0.05f;
    [Space(10)]
    [SerializeField, ColorUsage(false, false)] private Color baseColor;
    [SerializeField, ColorUsage(false, false)] private Color createColor;
    [SerializeField, ColorUsage(false, false)] private Color eraserColor;
    [Space(10)]
    [SerializeField, Range(0, 1.00f)] private float waterInfoOpacity = 0.33f;

    [Header("Internal")]
    private MaterialPropertyBlock gridPropBlock;
    private MaterialPropertyBlock bgPropBlock;
    [SerializeField] private float time = 0f;
    private bool eraser = false;

    private void Awake()
    {
        input = InputHandler.Instance;
    }

    void Start()
    {
        //permet d'overide les param sans modif le mat ou créer d'instance
        gridPropBlock = new MaterialPropertyBlock();
        //Recup Data
        gridRend.GetPropertyBlock(gridPropBlock);
        #region EditZone
        gridPropBlock.SetFloat("_Alpha", 0);
        gridPropBlock.SetFloat("_Thickness", 0);
        gridPropBlock.SetColor("_Color", baseColor);
        #endregion
        //Push Data
        gridRend.SetPropertyBlock(gridPropBlock);

        //permet d'overide les param sans modif le mat ou créer d'instance
        bgPropBlock = new MaterialPropertyBlock();
        //Recup Data
        bgRend.GetPropertyBlock(bgPropBlock);
        #region EditZone
        bgPropBlock.SetFloat("_Wireframe", 0);
        #endregion
        //Push Data
        bgRend.SetPropertyBlock(bgPropBlock);
    }

    void Update()
    {
        eraser = (input.mode == InputMode.eraser);
        //Input
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1) || 
           input.mode == InputMode.lake || input.mode == InputMode.cloud || input.mode == InputMode.source)
        {
            time += Time.deltaTime * (1.0f / durUp);
            time = Mathf.Clamp01(time);
        }
        else
        {
            time -= Time.deltaTime * (1.0f / durDown);
            time = Mathf.Clamp01(time);
        }

        #region grid
        //permet d'overide les param sans modif le mat ou créer d'instance
        gridPropBlock = new MaterialPropertyBlock();
        //Recup Data
        gridRend.GetPropertyBlock(gridPropBlock);
        //EditZone
        gridPropBlock.SetFloat("_Alpha", Mathf.Lerp(0, gridMaxOpacity, KarpEase.InOutSine(time)));
        gridPropBlock.SetFloat("_Thickness", Mathf.Lerp(0, gridMaxThickness, KarpEase.InOutCirc(time)));
        gridPropBlock.SetColor("_Color", Color.Lerp(baseColor, createColor, KarpEase.InOutSine(time)));
        //Push Data
        gridRend.SetPropertyBlock(gridPropBlock);
        #endregion

        #region BackGround
        //permet d'overide les param sans modif le mat ou créer d'instance
        bgPropBlock = new MaterialPropertyBlock();
        //Recup Data
        bgRend.GetPropertyBlock(bgPropBlock);
        //EditZone
        bgPropBlock.SetFloat("_Wireframe", Mathf.Lerp(0, waterInfoOpacity, KarpEase.InOutCubic(time)));
        //Push Data
        bgRend.SetPropertyBlock(bgPropBlock);
        #endregion

    }
}
