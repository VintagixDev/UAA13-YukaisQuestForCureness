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

    // G�n�re le Donjon en arri�re plan : piece->porte
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

    // G�n�re le joueur en arri�re plan : Stat->camera
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

    // G�n�re l'interface en arri�re plan
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
        /// Cette partie sert � d�tecter si la pieces doit lancer la bataille
        /// </aside>
        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Room"); // Recherche de tout les objets "Room"
        foreach (GameObject piece in pieces)
        {
            Room room = piece.GetComponent<Room>(); // Attribution des composants de scripts "Room.cs"
            if (room.RoomID == gameStat.CurrentRoom) // V�rification si la pieces cible est celle ou le joueur est
            {
                if (room.isBattleFinished == false) // V�rification si la piece sert au combat ou non
                {
                    /// <aside>
                    /// Cette partie sert � d�tecter toute les portes de la pi�ce actuelle du joueur et de les fermer avant de lancer la bataille
                    /// </aside>
                    GameObject[] portes = GameObject.FindGameObjectsWithTag("Door"); // Recherche de tout les objets "Door"
                    foreach (GameObject porte in portes)
                    {
                        Door door = porte.GetComponent<Door>(); // Attribution des composants de scripts "Door.cs"
                        if (door._roomId == gameStat.CurrentRoom) // V�rification si les portes cible sont bien dans la piece cible
                        {
                            door.CloseUnClose(true); // Sert � changer les sprites de la piece
                            //Debug.Log("changement d'�tat de la porte");


                        }
                    }

                    /// <aside>
                    /// Cette partie sert � faire apparaitre les ennemis
                    /// </aside>
                    GameObject[] spawners = GameObject.FindGameObjectsWithTag("EnemySpawner");
                    foreach (GameObject spawner in spawners)
                    {
                        EnemySpawner enemySpawnerScript = spawner.GetComponent<EnemySpawner>();
                        if (enemySpawnerScript.RoomID == gameStat.CurrentRoom)
                        {
                            enemySpawnerScript.SpawnRandomEnemy();
                            Destroy(spawner);
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