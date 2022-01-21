using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderLink_Template : MonoBehaviour
{
    [Header("reference")]
    public Renderer rend;

    [Header("Parameter")]
    [SerializeField] private float param = 5f;

    [Header("Internal")]
    private MaterialPropertyBlock propBlock;

    void Start()
    {
        //permet d'overide les param sans modif le mat ou créer d'instance
        propBlock = new MaterialPropertyBlock();
        //Recup Data
        rend.GetPropertyBlock(propBlock);
        //Edit
        #region EditZone
        propBlock.SetFloat("_Param", param);
        #endregion
        //Push Data
        rend.SetPropertyBlock(propBlock);
    }

    void Update()
    {
        bool modify = true;
        if (modify)
        {
            //permet d'overide les param sans modif le mat ou créer d'instance
            propBlock = new MaterialPropertyBlock();
            //Recup Data
            rend.GetPropertyBlock(propBlock);
            //Edit
            #region EditZone
            propBlock.SetFloat("_Param", param);
            #endregion
            //Push Data
            rend.SetPropertyBlock(propBlock);
        }
    }
}
