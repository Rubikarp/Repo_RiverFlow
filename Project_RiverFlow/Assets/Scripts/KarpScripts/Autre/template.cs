using UnityEngine;

#pragma warning disable 0414

public class template : MonoBehaviour
{
    #region Variables
    [Header("Variable")]

    [Space(10)]

    [SerializeField, Tooltip("c'est ma booléen")]
    private bool myBool = true;

    [SerializeField, Min(0)]
    private int myInt = 0;

    [SerializeField, Range(0,100)] 
    private float myFloat = 0.0f;

    [SerializeField, TextArea(1,3)]
    private string myTrue = "hello world";

    [SerializeField]
    private Vector3 myVector3;

    [SerializeField, ColorUsage(true, false)]
    private Color myColor;

    public float Flote
    {
        get
        {
            return myFloat;
        }
        set
        {
            myFloat = value;
        }
    }

    #endregion

    #region Methodes

    [ContextMenu("MyMethode")]
    public void MyMethode()
    {
        myInt = 1;
    }

    private void Awake()
    {
        
    }
    private void Start()
    {
        
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }


    #endregion

}

//Si combinatoire [System.Flags]
public enum TemplateEnum
{
    A = 1,
    B = 2,
    C = 3,
    D = 4,
}

[System.Serializable]
struct templateStruct
{

    public templateStruct(string name)  //Constructor
    {

    }
}

#pragma warning restore 0414