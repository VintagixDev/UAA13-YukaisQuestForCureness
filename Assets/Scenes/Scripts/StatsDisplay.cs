using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI enemiesKilledText;
    public TextMeshProUGUI chestsOpenedText;
    public TextMeshProUGUI currentFloorText;
    //public TextMeshProUGUI difficultyLevelText;

    void Start()
    {
        enemiesKilledText.text = "Ennemis tues : " + GameStat.Instance.EnemiesKilled;
        chestsOpenedText.text = "Coffres ouverts : " + GameStat.Instance.ChestsOpened;
        currentFloorText.text = "Etage atteint : " + GameStat.Instance.CurrentFloor;
        //difficultyLevelText.text = "Difficulté : " + GameStat.Instance.DifficultyLevel;
    }

    public void UpdateCounter()
    {
        enemiesKilledText.text = "Ennemis tues : " + GameStat.Instance.EnemiesKilled;
        chestsOpenedText.text = "Coffres ouverts : " + GameStat.Instance.ChestsOpened;
        currentFloorText.text = "Etage atteint : " + GameStat.Instance.CurrentFloor;
        //difficultyLevelText.text = "Difficulté : " + GameStat.Instance.DifficultyLevel;
    }
}
