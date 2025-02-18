using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class GameStat : MonoBehaviour
{
    [Header("Pièce actuelle du joueur")]
    [SerializeField] public string CurrentRoom;

    [Header("")]
    [SerializeField] public int CurrentFloor;
    [SerializeField] public int EnemiesKilled;
    [SerializeField] public int ChestsOpened;
    [SerializeField] public int DifficultyLevel;

    public void ResetStatsForNewFloor()
    {
        EnemiesKilled = 0;
        ChestsOpened = 0;
        DifficultyLevel = CurrentFloor; // La difficulté augmente avec l'étage
    }
}
