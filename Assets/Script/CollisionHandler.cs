using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delay;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem explosionEffect;
    [SerializeField] ParticleSystem successEffect;

    Moving moving;
    AudioSource audioSource;

    bool collosionSwitch = true;

    // Start is called before the first frame update
    void Start()
    {
        moving = GetComponent<Moving>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Cheat();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collosionSwitch)
        {
            switch (collision.gameObject.tag)
            {
                case "Friendly":
                    {
                        break;
                    }
                case "Fuel":
                    {
                        
                        break;
                    }
                case "Finish":
                    {
                        StartSuccessSequence();
                        break;
                    }
                default:
                    StartCrashSequence();
                    break;
            } 
        }
    }

    private void StartCrashSequence()
    {
        explosionEffect.Play();
        moving.Stop();
        audioSource.PlayOneShot(death);
        collosionSwitch = false;
        moving.enabled = false;
        DestroyChild(this.transform);
        Invoke("ReloadLevel",delay);
    }

    private void StartSuccessSequence()
    {
        successEffect.Play();
        moving.Stop();
        audioSource.PlayOneShot(success);
        collosionSwitch = false;
        moving.enabled = false;
        Invoke("LoadNextLevel", delay);
    }

    private void ReloadLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        GameManager.ReloadScene();
    }

    private void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = 0;
        if (currentSceneIndex != SceneManager.sceneCountInBuildSettings-1)
        {
            nextLevelIndex = currentSceneIndex+1;
        }
        GameManager.LoadNextScene();
    }

    //private void Cheat()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //        LoadNextLevel();

    //    if (Input.GetKeyDown(KeyCode.C))
    //        collosionSwitch = !collosionSwitch;

    //    if (Input.GetKeyDown(KeyCode.R))
    //        ReloadLevel();
    //}

    private void DestroyChild(Transform t)
    {
        foreach (Transform child in t)
        {
            ParticleSystem ps = child.GetComponent<ParticleSystem>();

            if (ps == null)
                Destroy(child.gameObject);
        }
    }
}
