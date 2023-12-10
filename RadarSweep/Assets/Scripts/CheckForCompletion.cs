using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForCompletion : MonoBehaviour
{
    bool didCheck = false;

    void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            Invoke("CheckCompletion", 10f);
        }
        else if (SceneManager.GetActiveScene().buildIndex < 3)
        {
            Invoke("CheckCompletion", 30f);
        }
        else if (SceneManager.GetActiveScene().buildIndex >= 3)
        {
            Invoke("CheckCompletion", 5f);
        }
        else
        {
            Invoke("CheckCompletion", 5f);
        }
    }

    private void Update()
    {
        if (didCheck)
        {
            Invoke("CheckCompletion", 5f);
            didCheck = false;
        }
    }

    private void CheckCompletion()
    {
        GameObject[] blueTag = GameObject.FindGameObjectsWithTag("Blue");
        GameObject[] banditTag = GameObject.FindGameObjectsWithTag("Bandit");
        GameObject[] banditAiTag = GameObject.FindGameObjectsWithTag("BanditAi");

        if (banditTag.Length == 0 && banditAiTag.Length == 0)
        {
            //if current scene == scene count - credit scene
            if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            else
            {
                SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
            }
        }

        if (blueTag.Length == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        didCheck = true;
    }
}