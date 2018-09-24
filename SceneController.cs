using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
    public string sceneName;
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // loads current scene
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName); // loads current scene
    }
}
