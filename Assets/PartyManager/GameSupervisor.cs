using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
    [Header("Manager")]
    [SerializeField] private RoomManager roomManager;
    [SerializeField] private TeleportationManager teleportationManager;

    [Header("Stats")]
    [SerializeField] private GameStat gameStat;

    [Header("Player prefab")]
    [SerializeField] GameObject playerPrefab;
    private GameObject currentPlayer;

    private void Start()
    {
        if (InitializeComponents())
        {
            Debug.Log("Game Initialized Successfully");
        }
        else
        {
            Debug.LogWarning("Something Wrong here !");
        }
    }

    private bool InitializeComponents()
    {
        if (SetDungeon())
        {
            if (SetPlayer())
            {
                if (SetUI())
                {
                    return true;
                }
                else
                {
                    Debug.Log("Error 101: Ui can't be set");
                    return false;
                }
            }
            else
            {
                Debug.Log("Error 401: Player can't be set");
                return false;
            }
        }
        else
        {
            Debug.Log("Error 301: The entire dungeon can't be set");
            return false;
        }
    }

    // Génère le Donjon en arrière plan : piece->porte
    public bool SetDungeon()
    {
        if (roomManager != null)
        {
            roomManager.StartDungeonGeneration();
            return true;
        }
        else
        {
            Debug.LogError("RoomManager not assigned!");
            return false;
        }
    }

    // Génère le joueur en arrière plan : Stat->camera
    public bool SetPlayer()
    {
        Destroy(currentPlayer);
        if (playerPrefab != null)
        {
            currentPlayer = Instantiate(playerPrefab, new Vector2(0, 0), Quaternion.identity);
            currentPlayer.name = "Player";
            return true;
        }
        else
        {
            return false;
        }
    }

    // Génère l'interface en arrière plan
    public bool SetUI()
    {
        return true;
    }
    public void AdvanceToNextFloor()
    {
        Debug.Log("Advancing to next floor...");
        GenerateNewFloor();
    }
    public void GenerateNewFloor()
    {
        roomManager.ClearDungeon();
        //gameStat.CurrentFloor++;
        SetDungeon();
        //SetPlayer();
    }
    public void Battle()
    {
        /// <aside>
        /// Cette partie sert à détecter si la pieces doit lancer la bataille
        /// </aside>
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>();
            if (room.RoomID == gameStat.CurrentRoom)
            {
                if (room.isBattleFinished == false)
                {
                    /// <aside>
                    /// Cette partie sert à détecter toute les portes de la pièce actuelle du joueur et de les fermer avant de lancer la bataille
                    /// </aside>
                    GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
                    foreach (GameObject porte in portes)
                    {
                        Door door = porte.GetComponent<Door>();
                        if (door._roomId == gameStat.CurrentRoom)
                        {
                            door.CloseUnClose(true);
                        }
                    }

                    /// <aside>
                    /// Cette partie sert à faire apparaitre les ennemis
                    /// </aside>
                    GameObject[] spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
                    foreach (GameObject spawner in spawners)
                    {
                        EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
                        if (enemySpawnerScript.RoomID == gameStat.CurrentRoom)
                        {
                            enemySpawnerScript.SpawnRandomEnemy();
                        }
                    }
                }
            }
        }

        

    }
    public void FinishBattle()
    {
        //Si les ennemis sont tous mort
        //ouvrir les portes

        GameObject[] portes = GameObject.FindGameObjectsWithTag("Door");
        foreach (GameObject porte in portes)
        {
            Door door = porte.GetComponent<Door>();
            if (door._isLockedByBattle == true)
            {
                door.CloseUnClose(false);
            }
        }
    }
}