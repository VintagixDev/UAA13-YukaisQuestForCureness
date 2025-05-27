using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [Header("Private components")]
    [SerializeField]
    private GameObject _hud;
    [SerializeField]
    private StatsDisplay _stat;
    [SerializeField]
    private GameObject _player;

    [Header("Affichage des cœurs")]
    [SerializeField, Tooltip("Tableau des cœurs dans l'ordre")]
    private GameObject[] _hearts;
    [SerializeField, Tooltip("Le texte")] 
    private TextMeshProUGUI _text;   
    
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponents();
            if (isPaused)
            {
                if (_hud != null) _hud.SetActive(true);
                ResumeGame();
            }
            else
            {
                if (_hud != null) _hud.SetActive(false);
                updateDisplayHearts();
                PauseGame();
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        _hud.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void PauseGame()
    {
        _stat.UpdateCounter();
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ReloadGame()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    /// <summary>
    /// Actualise le nombre de coeurs
    /// </summary>
    private void updateDisplayHearts()
    {
        if (_player == null || _hearts == null || _text == null) return;

        int actualHp = _player.GetComponent<PlayerStats>().playerHP;
        int maxHearts = _hearts.Length;
        int visibleHearts = Mathf.Min(actualHp, maxHearts);

        for (int i = 0; i < maxHearts; i++)
        {
            _hearts[i].SetActive(i < visibleHearts);
        }

        if (actualHp > maxHearts)
        {
            _text.gameObject.SetActive(true);
            _text.text = "+" + (actualHp - maxHearts).ToString();
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Récupère les composants
    /// </summary>
    private void GetComponents()
    {
        if (_hud == null)
            _hud = GameObject.FindGameObjectWithTag("UI");

        if (_player == null)
            _player = GameObject.FindGameObjectWithTag("Player");

        GameObject statGO = GameObject.Find("STAT");
        if (statGO != null)
            _stat = statGO.GetComponent<StatsDisplay>();


    }
}
