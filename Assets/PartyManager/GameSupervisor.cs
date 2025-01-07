using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
    private string roomIdAtPlayer;
    
    private void Start()
    {
        if (InitializeComponents())
        {
            // Lancer le jeu ^^
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
        return true;
    }

    // G�n�re le joueur en arri�re plan : Stat->ui
    public bool SetPlayer()
    {
        return true;
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