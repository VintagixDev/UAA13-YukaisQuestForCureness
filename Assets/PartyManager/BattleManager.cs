using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Other class")]
    [SerializeField] private GameStat gameStat;

    public int _remainingEnemies = 0; // Besoin que ce soit public

    private void Update()
    {
        //if (_remainingEnemies < 0)
        //{
        //    GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //    foreach (GameObject enemie in enemies)
        //    {
        //        Destroy(enemie);
        //    }
        //    _remainingEnemies = 0;
        //}
    }

    public void AddEnemiesCount()
    {
        _remainingEnemies++;
    }
    public void RemoveEnemiesCount()
    {
        _remainingEnemies--;
    }

    /// <summary>
    /// Méthode servant à lancer le combat
    /// </summary>
    public void BattlePro()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>();
            if (room.RoomID == gameStat.CurrentRoom)
            {
                if (!room.isBattleFinished)
                {
                    // Ferme les portes avant de commencer le combat
                    CloseDoorsForBattle();

                    // Fait apparaître les ennemis
                    SpawnEnemies();
                }
            }
        }
    }

    /// <summary>
    /// Met fin à la bataille en cours
    /// </summary>
    public void FinishBattleMethod()
    {
        if (_remainingEnemies == 0)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemie in enemies)
            {
                Destroy(enemie);
            }
            // Ouvre ttes les portes à la fin du combat
            OpenDoorsCurrentRoom();
            // Change les réglages de la pièce
            ChangeRoomSettings();
        }
    }

    /// <summary>
    /// Ferme les portes de la pièce concerné 
    /// </summary>
    private void CloseDoorsForBattle()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door.RoomID == gameStat.CurrentRoom)
            {
                door.CloseUnClose(true); // Ferme la porte
            }
        }
    }

    /// <summary>
    /// Fait apparaitre les ennemis
    /// </summary>
    private void SpawnEnemies()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject spawner in spawners)
        {
            EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
            if (enemySpawnerScript.RoomID == gameStat.CurrentRoom)
            {
                enemySpawnerScript.SpawnRandomEnemy(); // Fait apparaitre aléatoirement un ennemis
                Destroy(spawner); // Détruit l'objet d'apparition
            }
        }
    }

    /// <summary>
    /// Change les réglages de la pièce
    /// </summary>
    private void ChangeRoomSettings()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>(); // Récupère le composant "Room.cs"
            if (room.RoomID == gameStat.CurrentRoom) // Vérifie si la pièce est celle du joueur
            {
                room.isBattleFinished = true; // La pièce passe en combat à eu lieu
            }
        }
    }

    /// <summary>
    /// Ouvre ttes les portes à la fin du combat
    /// (pas besoin de vérifier la salle vu que les seuls fermées sont celle de la pièce concerné)
    /// </summary>
    private void OpenDoorsCurrentRoom()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door.IsLockedByBattle() && door.RoomID == gameStat.CurrentRoom) // est verrouillée par la bataille et fait partie de la pièce
            {
                door.CloseUnClose(false); // Ouvre la porte
            }
        }
    }
}