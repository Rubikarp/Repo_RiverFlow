using UnityEngine;
using UnityEngine.SceneManagement;
using NaughtyAttributes;

public class RforReset : MonoBehaviour
{
    [Scene]
    public int scene;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(scene);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

    }
}
