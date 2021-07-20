using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageText : MonoBehaviour
{
    Text text;
    // Start is called before the first frame update
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int levelID = currentScene.buildIndex;
        string levelName = currentScene.name;
        text = gameObject.GetComponent<Text>();
        text.text = $"1-{levelID+1} {levelName}";
    }
}
