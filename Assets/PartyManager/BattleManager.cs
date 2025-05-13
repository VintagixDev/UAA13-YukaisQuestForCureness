using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Other class")]
    public GameStat gameStat;

    // Méthode de combat
    public void BattlePro()
    {
        // Recherche de toutes les pièces
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>(); // Récupère le composant "Room.cs"
            if (room.RoomID == gameStat.CurrentRoom) // Vérifie si la pièce est celle du joueur
            {
                if (!room.isBattleFinished) // Vérifie si la pièce n'est pas déjà utilisée pour un combat
                {
                    // Ferme les portes avant de commencer le combat
                    CloseDoorsForBattle();

                    // Fait apparaître les ennemis
                    SpawnEnemies();

                    // Lance le processus d'attente de la fin du combat
                    StartCoroutine(WaitForBattleToEnd());
                }
            }
        }
    }

    // Ferme toutes les portes de la pièce avant le combat
    private void CloseDoorsForBattle()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door._roomId == gameStat.CurrentRoom) // Si la porte est dans la pièce actuelle du joueur
            {
                door.CloseUnClose(true); // Ferme la porte
            }
        }
    }

    // Fait apparaître des ennemis dans la salle du joueur
    private void SpawnEnemies()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject spawner in spawners)
        {
            EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
            if (enemySpawnerScript.RoomID == gameStat.CurrentRoom)
            {
                enemySpawnerScript.SpawnRandomEnemy(); // Spawner un ennemi aléatoire
                Destroy(spawner); // Supprime le spawner une fois qu'il a servi
            }
        }
    }

    // Coroutine pour attendre la fin du combat
    private IEnumerator WaitForBattleToEnd()
    {
        float timeout = 30f; // Temps d'attente maximal (en secondes)
        float timer = 0f;

        while (!FinishBattle() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null; // Attendre la prochaine frame
        }

        if (timer >= timeout)
        {
            Debug.Log("Timeout de la bataille.");
        }
    }

    // Méthode pour vérifier si tous les ennemis sont morts
    public bool FinishBattle()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool allEnemiesDead = true;

        //foreach (GameObject enemy in enemies)
        //{
        //    RangedEnemy enemyScript = enemy.GetComponent<RangedEnemy>();
        //    if (enemyScript.isAlive) // Si un ennemi est encore vivant
        //    {
        //        allEnemiesDead = false;
        //        break;
        //    }
        //}

        if (allEnemiesDead)
        {
            OpenDoors(); // Ouvre les portes
            Debug.Log("Tous les ennemis sont morts. Les portes sont ouvertes.");
            return true;
        }
        else
        {
            Debug.Log("Il reste des ennemis à tuer.");
            return false;
        }
    }

    // Ouvre toutes les portes après la fin du combat
    void OpenDoors()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door._isLockedByBattle) // Si la porte est verrouillée par la bataille
            {
                door.CloseUnClose(false); // Ouvre la porte
            }
        }
    }
}