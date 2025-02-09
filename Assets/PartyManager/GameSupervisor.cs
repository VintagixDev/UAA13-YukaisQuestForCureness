using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
    [Header("Other class")]
    [SerializeField] private RoomManager roomManager;

    [Header("a")]
    [SerializeField] string roomIdAtPlayer;
    [SerializeField] bool BattleStarted;

    [Header("Player prefab")]
    [SerializeField] GameObject playerPrefab; 

    private void Start()
    {
        if (InitializeComponents())
        {

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
            roomManager.StartDungeonGeneration(); // On déclenche la génération
            return true;
        }
        else
        {
            Debug.LogError("RoomManager non assigné !");
            return false;
        }
    }

    // Génère le joueur en arrière plan : Stat->camera
    public bool SetPlayer()
    {
        GameObject playerToInstantiate = playerPrefab;
        if(playerToInstantiate != null)
        {
            GameObject player = Instantiate(playerToInstantiate ,new Vector2(0,0), Quaternion.identity);
            player.name = $"Player";
            return true;
        } else
        {
            return false;
        }
        
    }

    // Génère l'interface en arrière plan
    public bool SetUI()
    {
        return true;
    }
    public void EndBattle()
    {
        if (IsBattleIsFinished())
        {
            
        }
    }
    public bool IsBattleIsFinished()
    {

        return true;
    }
}