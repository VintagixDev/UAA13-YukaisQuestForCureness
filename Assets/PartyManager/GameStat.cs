using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    [Header("Actual room of player")]
    [Tooltip("Pièce actuel (id)")]
    [SerializeField] public int CurrentRoom; 

    [Header("Game progress")]
    [Tooltip("Etage actuel")]
    [SerializeField] public int CurrentFloor; 
    [Tooltip("Nb d'ennemis tués")]
    [SerializeField] public int EnemiesKilled;
    [Tooltip("Nb de coffres ouverts")]
    [SerializeField] public int ChestsOpened;
    [Tooltip("Niv de difficulté")]
    [SerializeField] public int DifficultyLevel;

    //[Header("Actual stats : playable part")]
    //[SerializeField] 

    public void ResetStatsForNewFloor()
    {
        EnemiesKilled = 0;
        ChestsOpened = 0;
        DifficultyLevel = 0;
        CurrentFloor = 0;
    }
}
