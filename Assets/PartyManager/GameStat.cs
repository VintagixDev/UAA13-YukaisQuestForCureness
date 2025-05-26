using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    public static GameStat Instance;

    [Header("Actual room of player")]
    public int CurrentRoom;

    [Header("Game progress")]
    public int CurrentFloor;
    public int EnemiesKilled;
    public int ChestsOpened;
    public int DifficultyLevel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameStat instance créée");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ResetStatsForNewFloor()
    {
        EnemiesKilled = 0;
        ChestsOpened = 0;
        DifficultyLevel = 0;
        CurrentFloor = 0;
    }
}
