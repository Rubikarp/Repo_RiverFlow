using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "new level", menuName = "Level")]
public class LevelSO : ScriptableObject
{
    public string levelSceneName;
    public Sprite levelCapture;
    public int levelNumber;
    public new string name;
    //public string description;
    public int recordScore;
    public int scoreUnlock;
    public LevelSO LockLvl;

}
