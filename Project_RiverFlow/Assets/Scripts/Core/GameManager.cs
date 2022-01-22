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
        if (levelSaves == null)
        {
            Debug.Log("no save bru");
            //creer des fichiers ou tt les scores max sont à 0
            levelSaves = new List<LevelSave>();
            if (levelSaves.Count != levelList.Count)
            {
                for (int i = 0; i < levelList.Count; i++)
                {
                    levelSaves.Add(new LevelSave(0));
                }
                //on les save
                Save.SaveLevel(levelSaves);
            }
        }
    }

    public static void ChangeScene(string sceneName)
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
