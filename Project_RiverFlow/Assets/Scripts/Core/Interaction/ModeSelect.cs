using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public enum Items
{
    Dig,
    Cloud,
    Source,
    Lake,
    Tunnel,
}

public class ModeSelect : MonoBehaviour,
    IDragHandler,
    IBeginDragHandler,
    IEndDragHandler
{
    [Header("Component")]
    public InputHandler input;
    public InventoryManager inventory;
    [Space(5)]
    public TextMeshProUGUI textGUI;
    private Image SelfImage;
    [Space(5)]
    [SerializeField] private Sprite on;
    [SerializeField] private Sprite off;

    [Header("Parameter")]
    [SerializeField] private InputMode mode;

    private void Start()
    {
        input = InputHandler.Instance;
        inventory = GetComponent<InventoryManager>();
    }

    //Call Once
    public void OnBeginDrag(PointerEventData eventData)
    {
        input.mode = mode;
    }

    //Call on Event update
    public void OnDrag(PointerEventData eventData)
    {

    }

    //Call Once
    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
