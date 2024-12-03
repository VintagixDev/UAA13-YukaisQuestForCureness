using UnityEngine;

public class GameSupervisor : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        if (InitializeComponents())
        {
            //Lancer le jeu ^^
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

    // Génére le Donjon en arrière plan : piece->porte
    public bool SetDungeon()
    {
        return true;
    }

    // Génére le joueur en arrière plan : Stat->ui
    public bool SetPlayer()
    {
        return true;
    }

    // Génére l'interface en arrière plan
    public bool SetUI()
    {
        return true;
    }
}