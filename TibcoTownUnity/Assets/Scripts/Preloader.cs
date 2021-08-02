using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Preloader : MonoBehaviour
{
    [SerializeField] private Image fill;

    private void Start()
    {
        StartCoroutine(LoadGameScene());
    }

    IEnumerator LoadGameScene()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("Game");
        while (!asyncLoad.isDone)
        {
            fill.fillAmount = asyncLoad.progress;
            yield return new WaitForEndOfFrame();
        }
        yield return SceneManager.UnloadSceneAsync("Loading");
    }
}
