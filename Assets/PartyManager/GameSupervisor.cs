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
        //Obtenir l'ID de la piece du joueur
        
        int CurrentRoomMaster = gameStat.CurrentRoom;

        //Rechercher l'objet par son nom
        GameObject RoomTempOBJMaster = GameObject.Find("ROOM-"+CurrentRoomMaster); // �a doit etre l'objet Room
        
        if (RoomTempOBJMaster != null)
        {
            Room roomScript = RoomTempOBJMaster.GetComponent<Room>();
            if (roomScript.isBattleFinished == false) // Si aucun combat n'a encore eu lieu
            {
                //Fermer les portes
                foreach(GameObject enemySpawner in roomScript.enemySpawners)
                {
                    EnemySpawner enemySpawnerScript = enemySpawner.GetComponent<EnemySpawner>();
                    Debug.Log(enemySpawner);
                    enemySpawnerScript.SpawnRandomEnemy();
                }

            } else // Si le combat a d�j� eu lieu 
            {
                //Rien faire
            }
        }
    }
    public void FinishBattle()
    {
        //Si les ennemis sont tous mort
        //ouvrir les portes
    }
}