using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    GameObject[] gameObjects;

    static int _currentLevelIndex;
    static bool _thisSceneIsFirstLoad = true;
    static public bool isHide;

    public static bool ThisSceneIsFirstLoad
    {
        get { return _thisSceneIsFirstLoad; }
    }

    public void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else if (instance != this)
            instance = this;

        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        Cursor.visible = false;
    }

    private void Start()
    {
        gameObjects = GameObject.FindGameObjectsWithTag("DontDestroyOnLoad");
        foreach (var item in gameObjects)
        {
            DontDestroyOnLoad(item);
        }
    }

    public static void TurnToScanMode()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>().enabled = false;
    }

    public static void TurnToNormal()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Moving>().enabled = true;
    }

    public static void ReloadScene()
    {
        _thisSceneIsFirstLoad = false;
        SceneManager.LoadScene(_currentLevelIndex);
    }

    public static void LoadNextScene()
    {
        _thisSceneIsFirstLoad = true;
        _currentLevelIndex++;
        if (_currentLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 0;
        }
        else
            SceneManager.LoadScene(_currentLevelIndex);
    }
}
