using UnityEngine.EventSystems;
using UnityEngine.Events;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EventTrigger))]
public class KarpButton : MonoBehaviour
{
    [Header("Reference")]
    private RectTransform self;
    private EventTrigger trigger;
    [SerializeField] private TextMeshProUGUI textZone;
    [SerializeField] private Image selfImage;

    [Header("Variable")]
    public UnityEvent buttonClick;
    [Header("Color")]
    [ColorUsage(true, false)] public Color hoverColor = Color.grey;
    [ColorUsage(true, false)] public Color unhoverdeColor = Color.white;
    [ColorUsage(true, false)] public Color clickedColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    private void Awake()
    {
        trigger = transform.GetComponent<EventTrigger>();
        self = transform.GetComponent<RectTransform>();
    }

    void Update()
    {

    }

    public void OnHover()
    {
        selfImage.color = hoverColor;
    }
    public void OnUnhover()
    {
        selfImage.color = unhoverdeColor;
    }
    public void OnClick()
    {
        selfImage.color = clickedColor;
    }

    /*public void OnDrag(BaseEventData bed)
    {
        PointerEventData ped = bed as PointerEventData;
        self.anchoredPosition += ped.delta;
    }
    */
}
