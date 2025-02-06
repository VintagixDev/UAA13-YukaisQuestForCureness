using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
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
        return true;
    }

    // Génère le joueur en arrière plan : Stat->ui
    public bool SetPlayer()
    {
        GameObject playerToInstantiate = playerPrefab;

        //GameObject player = Instantiate(playerToInstantiate, , Quaternion.identity);
        //player.name = $"PLAYER";
        return true;
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