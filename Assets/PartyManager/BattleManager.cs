using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Other class")]
    public GameStat gameStat;

    // M�thode de combat
    public void BattlePro()
    {
        // Recherche de toutes les pi�ces
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>(); // R�cup�re le composant "Room.cs"
            if (room.RoomID == gameStat.CurrentRoom) // V�rifie si la pi�ce est celle du joueur
            {
                if (!room.isBattleFinished) // V�rifie si la pi�ce n'est pas d�j� utilis�e pour un combat
                {
                    // Ferme les portes avant de commencer le combat
                    CloseDoorsForBattle();

                    // Fait appara�tre les ennemis
                    SpawnEnemies();

                    // Lance le processus d'attente de la fin du combat
                    StartCoroutine(WaitForBattleToEnd());
                }
            }
        }
    }

    // Ferme toutes les portes de la pi�ce avant le combat
    private void CloseDoorsForBattle()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door._roomId == gameStat.CurrentRoom) // Si la porte est dans la pi�ce actuelle du joueur
            {
                door.CloseUnClose(true); // Ferme la porte
            }
        }
    }

    // Fait appara�tre des ennemis dans la salle du joueur
    private void SpawnEnemies()
    {
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
        foreach (GameObject spawner in spawners)
        {
            EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
            if (enemySpawnerScript.RoomID == gameStat.CurrentRoom)
            {
                enemySpawnerScript.SpawnRandomEnemy(); // Spawner un ennemi al�atoire
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

    // M�thode pour v�rifier si tous les ennemis sont morts
    public bool FinishBattle()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool allEnemiesDead = true;

        foreach (GameObject enemy in enemies)
        {
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript.isAlive) // Si un ennemi est encore vivant
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (allEnemiesDead)
        {
            OpenDoors(); // Ouvre les portes
            Debug.Log("Tous les ennemis sont morts. Les portes sont ouvertes.");
            return true;
        }
        else
        {
            Debug.Log("Il reste des ennemis � tuer.");
            return false;
        }
    }

    // Ouvre toutes les portes apr�s la fin du combat
    void OpenDoors()
    {
        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door._isLockedByBattle) // Si la porte est verrouill�e par la bataille
            {
                door.CloseUnClose(false); // Ouvre la porte
            }
        }
    }
}