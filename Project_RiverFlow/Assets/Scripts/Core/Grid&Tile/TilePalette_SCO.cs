using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "TilePalette_new", menuName = "GraphData/TilePalette")]
public class TilePalette_SCO : ScriptableObject
{
    [Header("Ground")]
    [ColorUsage(true, false)] public Color holedGround = Color.gray;
    [Space(5)]
    [ColorUsage(true, false)] public Color groundGrass = Color.green;
    [ColorUsage(true, false)] public Color groundClay = Color.red;
    [ColorUsage(true, false)] public Color groundAride = Color.yellow;
    [ColorUsage(true, false)] public Color mountain = Color.gray;

    [Header("BackUp")]
    [ColorUsage(true, false)] public Color errorMat = Color.magenta;

}
