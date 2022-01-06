using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentModVisualizer : MonoBehaviour
{
    public RectTransform dig;
    public RectTransform eraser;
    public RectTransform cloud;
    public RectTransform lake;
    public RectTransform source;
    private InputHandler input;
    public float sizeAugment =1.2f;
    // Start is called before the first frame update
    void Awake()
    {
        input = InputHandler.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        switch (input.mode)
        {
            case InputMode.nothing:
                dig.localScale =Vector3.one*1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.diging:
                dig.localScale = Vector3.one * sizeAugment;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.eraser:
                dig.localScale = Vector3.one * 1;
                eraser.localScale = Vector3.one * sizeAugment;
                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.cloud:
                dig.localScale = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale = Vector3.one * sizeAugment;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.lake:
                dig.localScale = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * sizeAugment;
                source.localScale = Vector3.one * 1;
                break;
            case InputMode.source:
                dig.localScale = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * sizeAugment;
                break;
            default:
                dig.localScale = Vector3.one * 1;
                eraser.localScale = Vector3.one * 1;
                cloud.localScale = Vector3.one * 1;
                lake.localScale = Vector3.one * 1;
                source.localScale = Vector3.one * 1;
                break;
        }

    }
}
