using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    [Header("Actual room of player")]
    [SerializeField] public int CurrentRoom; //Id de la pièces actuelle du joueur

    [Header("")]
    [SerializeField] public int CurrentFloor;
    [SerializeField] public int EnemiesKilled;
    [SerializeField] public int ChestsOpened;
    [SerializeField] public int DifficultyLevel;

    public void ResetStatsForNewFloor()
    {
        EnemiesKilled = 0;
        ChestsOpened = 0;
        // La difficulté augmente avec l'étage
        DifficultyLevel = 0;
        CurrentFloor = 0; 
    }
}
