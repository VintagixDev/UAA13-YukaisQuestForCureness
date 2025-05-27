using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{


    ManagerScene sceneManager = new ManagerScene();


    public void onClickPlayButton()
    {
        
        StartCoroutine(sceneManager.LoadAsyncScene(ManagerScene.Scenes.GameScene));
    }

    public void onClickQuitButton()
    {
        Application.Quit();
    }

    public void onClickReturnToTitle()
    {
        StartCoroutine(sceneManager.LoadAsyncScene(ManagerScene.Scenes.MainMenu));
    }

    public void OnClickTutoButton()
    {
        StartCoroutine(sceneManager.LoadAsyncScene(ManagerScene.Scenes.TutoScene));
    }

}
