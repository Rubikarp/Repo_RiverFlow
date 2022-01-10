using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public List<LevelSO> levelList;
    [HideInInspector]
    public List<LevelSave> levelSaves;

    void Start()
    {
        MakeSingleton(true);
        levelSaves = Save.LoadSave();
        //si on a pas de saves
        if (levelSaves.Count < levelList.Count)
        {
            //creer des fichiers ou tt les scores max sont � 0
            levelSaves = new List<LevelSave>();
            for (int i=0; i<levelList.Count; i++)
            {
                levelSaves.Add(new LevelSave(0));
            }
            //on les save
            Save.SaveLevel(levelSaves);
        }
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void SaveLevels()
    {
        Save.SaveLevel(levelSaves);
    }
}
