using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Buttons : MonoBehaviour
{
    ManagerScene sceneManager = new ManagerScene();

    [Header("UI Panels")]
    public GameObject menuPanel;
    public GameObject mainMenu;

    void Update()
    {
        if (menuPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            menuPanel.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

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

    public void OpenCreditsMenu()
    {
        mainMenu.SetActive(false);
        menuPanel.SetActive(true); 
    }
}
