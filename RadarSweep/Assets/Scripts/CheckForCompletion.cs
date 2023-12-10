using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckForCompletion : MonoBehaviour
{
    bool didCheck = false;

    void Start()
    {
        Invoke("CheckCompletion", 5f);
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
        GameObject[] banditTag = GameObject.FindGameObjectsWithTag("Bandit");
        GameObject[] banditAiTag = GameObject.FindGameObjectsWithTag("BanditAi");

        if (banditTag.Length == 0 && banditAiTag.Length == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        didCheck = true;
    }
}
