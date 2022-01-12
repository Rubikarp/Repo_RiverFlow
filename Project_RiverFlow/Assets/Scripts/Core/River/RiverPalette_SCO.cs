using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "RiverPalette_new", menuName = "GraphData/RiverPalette")]
public class RiverPalette_SCO : ScriptableObject
{
    [Header("River Tickness")]
    [Range(0f, 1f)] public float debit_00 = 1;
    [Range(0f, 1f)] public float debit_25 = 0.4f;
    [Range(0f, 1f)] public float debit_50 = 0.6f;
    [Range(0f, 1f)] public float debit_75 = 0.8f;
    [Range(0f, 1f)] public float debit_100 = 1;

    [Header("River Palette")]
    [ColorUsage(true, false)] public Color color_00 = Color.blue;
    [ColorUsage(true, false)] public Color color_25 = Color.blue;
    [ColorUsage(true, false)] public Color color_50 = Color.blue;
    [ColorUsage(true, false)] public Color color_75 = Color.blue;
    [ColorUsage(true, false)] public Color color_100 = Color.blue;

    [Header("BackUp")]
    [ColorUsage(true, false)] public Color errorMat = Color.magenta;

}
