using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    [Header("Actual room of player")]
    [SerializeField] public string CurrentRoom; //Id de la pi�ces actuelle du joueur

    [Header("")]
    [SerializeField] public int CurrentFloor;
    [SerializeField] public int EnemiesKilled;
    [SerializeField] public int ChestsOpened;
    [SerializeField] public int DifficultyLevel;

    public void ResetStatsForNewFloor()
    {
        EnemiesKilled = 0;
        ChestsOpened = 0;
        // La difficult� augmente avec l'�tage
        DifficultyLevel = 0;
        CurrentFloor = 0; 
    }
}
