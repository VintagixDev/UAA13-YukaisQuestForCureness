using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
    [Header("Manager")]
    [Tooltip("Script qui s'occupe de la g�n�ration proc�durale")]
    [SerializeField] private RoomManager roomManager;
    [Tooltip("Script qui s'occupe du d�placement du joueur dans le donjon")]
    [SerializeField] private TeleportationManager teleportationManager;
    [Tooltip("Script qui s'occupe des batailles")]
    [SerializeField] public BattleManager battleManager;

    [Header("Stats")]
    [Tooltip("Script qui contient les statistiques propres � la partie")]
    [SerializeField] private GameStat gameStat;

    [Header("Player prefab")]
    [Tooltip("Objet Joueur")]
    [SerializeField] GameObject playerPrefab;
    private GameObject currentPlayer;

    [Header("HUD prefab")]
    [Tooltip("Objet HUD")]
    [SerializeField] GameObject hud;
    private GameObject currentHud;

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
    public bool SetUI()
    {
        if (hud != null)
        {
            Destroy(currentHud); // Supprime l'ancien HUD si pr�sent
            currentHud = Instantiate(hud);
            currentHud.name = "HUD";
            return true;
        }
        else
        {
            Debug.LogError("HUD prefab not assigned!");
            return false;
        }
    }
    public void AdvanceToNextFloor()
    {
        Debug.Log("Advancing to next floor...");
        GenerateNewFloor();
    }
    public void GenerateNewFloor()
    {
        roomManager.ClearDungeon();
        SetDungeon();
    }
    public void Battle()
    {
        // Appel de la m�thode BattlePro() du BattleManager pour d�marrer le combat
        if (battleManager != null)
        {
            battleManager.BattlePro();
        }
        else
        {
            Debug.LogWarning("BattleManager non assign� !");
        }
    }

    ///
    /// Ensembles de proc�dures et fonctions servant � recharger les donn�es [Ne pas toucher en pleine partie sau � des fins de TEST]
    ///

    // Recharge uniquement l'�tage courant (sans changer l'�tage)
    public void ReloadFloor()
    {
        Debug.Log("Reloading current floor...");
        ResetScene();
    }

    // Red�marre le jeu � z�ro
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        if (gameStat != null)
        {
            gameStat.CurrentFloor = 0;
        }
        ResetScene();
    }

    // Supprime tout puis r�instancie
    private void ResetScene()
    {
        roomManager.ClearDungeon();
        Destroy(currentPlayer);
        Destroy(currentHud);

        InitializeComponents();
    }

}