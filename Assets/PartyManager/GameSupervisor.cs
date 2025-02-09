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

    // G�n�re le Donjon en arri�re plan : piece->porte
    public bool SetDungeon()
    {
        if (roomManager != null)
        {
            roomManager.StartDungeonGeneration(); // On d�clenche la g�n�ration
            return true;
        }
        else
        {
            Debug.LogError("RoomManager non assign� !");
            return false;
        }
    }

    // G�n�re le joueur en arri�re plan : Stat->camera
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

    // G�n�re l'interface en arri�re plan
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