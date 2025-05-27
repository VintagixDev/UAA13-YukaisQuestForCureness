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
    /// M�thode servant � lancer le combat
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

                    // Fait appara�tre les ennemis
                    SpawnEnemies();
                }
            }
        }
    }

    /// <summary>
    /// Met fin � la bataille en cours
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
            // Ouvre ttes les portes � la fin du combat
            OpenDoorsCurrentRoom();
            // Change les r�glages de la pi�ce
            ChangeRoomSettings();
        }
    }

    /// <summary>
    /// Ferme les portes de la pi�ce concern� 
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
                enemySpawnerScript.SpawnRandomEnemy(); // Fait apparaitre al�atoirement un ennemis
                Destroy(spawner); // D�truit l'objet d'apparition
            }
        }
    }

    /// <summary>
    /// Change les r�glages de la pi�ce
    /// </summary>
    private void ChangeRoomSettings()
    {
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>(); // R�cup�re le composant "Room.cs"
            if (room.RoomID == gameStat.CurrentRoom) // V�rifie si la pi�ce est celle du joueur
            {
                room.isBattleFinished = true; // La pi�ce passe en combat � eu lieu
            }
        }
    }

    /// <summary>
    /// Ouvre ttes les portes � la fin du combat
    /// (pas besoin de v�rifier la salle vu que les seuls ferm�es sont celle de la pi�ce concern�)
    /// </summary>
    private void OpenDoorsCurrentRoom()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door.IsLockedByBattle() && door.RoomID == gameStat.CurrentRoom) // est verrouill�e par la bataille et fait partie de la pi�ce
            {
                door.CloseUnClose(false); // Ouvre la porte
            }
        }
    }
}