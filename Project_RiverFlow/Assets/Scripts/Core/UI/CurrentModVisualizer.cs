using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentModVisualizer : MonoBehaviour
{
    [Header("Components")]
    private InputHandler input;
    [Space(5)]
    public RectTransform dig;
    public RectTransform eraser;
    public RectTransform cloud;
    public RectTransform lake;
    public RectTransform source;

    [Header("Parameters")]
    public float sizeAugment = 1.2f;
    [Header("Sounds")]
    public string modeButtonSound = "ModeButton";

    void Awake()
    {
        input = InputHandler.Instance;
    }

    void Update()
    {


        switch (input.mode)
        {
            case InputMode.nothing:
                dig.localScale    = Vector3.one*1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale  = Vector3.one * 1;
                lake.localScale   = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.diging:
                dig.localScale    = Vector3.one * sizeAugment;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale  = Vector3.one * 1;
                lake.localScale   = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.eraser:
                dig.localScale    = Vector3.one * 1;
                eraser.localScale = Vector3.one * sizeAugment;
                cloud.localScale  = Vector3.one * 1;
                lake.localScale   = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.cloud:
                dig.localScale    = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale  = Vector3.one * sizeAugment;
                lake.localScale   = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.lake:
                dig.localScale    = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale  = Vector3.one * 1;
                lake.localScale   = Vector3.one * sizeAugment;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.source:
                dig.localScale    = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale  = Vector3.one * 1;
                lake.localScale   = Vector3.one * 1;
                source.localScale = Vector3.one * sizeAugment;
                break;
            default:
                dig.localScale    = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale  = Vector3.one * 1;
                lake.localScale   = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
        }
            
        if (input.mode != InputMode.eraser)
        {
            if (Input.GetMouseButton(1))
            {
                eraser.localScale = Vector3.one * sizeAugment;
                dig.localScale = Vector3.one * 1;

                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
            }
            else
            {
                eraser.localScale = Vector3.one * 1;
            }
        }
    }

    public void playModeButtonSound()
    {
        LevelSoundboard.Instance.PlayModeUISound(modeButtonSound);
    }
}
