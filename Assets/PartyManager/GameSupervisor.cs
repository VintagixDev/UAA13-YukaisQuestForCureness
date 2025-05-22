using Unity.VisualScripting;
using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
    [Header("Manager")]
    [Tooltip("Script qui s'occupe de la génération procédurale")]
    [SerializeField] private RoomManager roomManager;
    [Tooltip("Script qui s'occupe du déplacement du joueur dans le donjon")]
    [SerializeField] private TeleportationManager teleportationManager;
    [Tooltip("Script qui s'occupe des batailles")]
    [SerializeField] private BattleManager battleManager;

    [Header("Stats")]
    [Tooltip("Script qui contient les statistiques propres à la partie")]
    [SerializeField] private GameStat gameStat;

    [Header("Player prefab")]
    [Tooltip("Objet Joueur")]
    public GameObject playerPrefab; // Sert de template, pas besoin de mettre en private
    private GameObject currentPlayer;

    [Header("HUD prefab")]
    [Tooltip("Objet HUD")]
    public GameObject hud; // Sert de template, pas besoin de mettre en private
    private GameObject currentHud;
    public GameObject bossUI;

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
    private bool SetDungeon()
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
    private bool SetPlayer()
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
    private bool SetUI()
    {
        if (hud != null)
        {
            Destroy(currentHud); // Supprime l'ancien HUD si présent
            currentHud = Instantiate(hud);
            currentHud.name = "HUD";
            bossUI = GameObject.FindGameObjectWithTag("bossUI");
            bossUI.SetActive(false);

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
        // Appel de la méthode BattlePro() du BattleManager pour démarrer le combat
        if (battleManager != null)
        {
            battleManager.BattlePro();
        }
        else
        {
            Debug.LogWarning("BattleManager non assigné !");
        }
    }

    ///
    /// Ensembles de procédures et fonctions servant à recharger les données [Ne pas toucher en pleine partie sau à des fins de TEST]
    ///

    // Recharge uniquement l'étage courant (sans changer l'étage)
    public void ReloadFloor()
    {
        Debug.Log("Reloading current floor...");
        ResetScene();
    }

    // Redémarre le jeu à zéro
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        if (gameStat != null)
        {
            gameStat.CurrentFloor = 0;
        }
        ResetScene();
    }

    // Supprime tout puis réinstancie
    private void ResetScene()
    {
        roomManager.ClearDungeon();
        Destroy(currentPlayer);
        Destroy(currentHud);

        InitializeComponents();
    }

}