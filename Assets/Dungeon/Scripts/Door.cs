using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private TeleportationManager teleportationManager; // Script de gestion des d�placement dans le donjon
    [SerializeField] private GameStat gameStat; // Script des stats de partie

    [Header("ID")]
    [SerializeField] public string id;

    [Header("Point de position")]
    [SerializeField] public float x;
    [SerializeField] public float y;
    [SerializeField] public float z;

    [Header("Orientation de la porte")]
    [SerializeField] public string orientation;

    [Header("Sprites des portes")]
    // Index 0:North / 1:South / 2:East / 3:West
    [SerializeField] public List<Sprite> spriteOpen;
    [SerializeField] public List<Sprite> spriteClose;
    [SerializeField] public List<Sprite> spriteLock;
    [SerializeField] public List<Sprite> spriteBossOpen;
    [SerializeField] public List<Sprite> spriteBossClose;

    private SpriteRenderer spriteRenderer;

    [SerializeField] public bool isLocked = false;
    [SerializeField] public bool isLockedByBattle = false;
    [SerializeField] public bool isBossDoor = false;

    public Vector2Int connectedDoorPosition;// Coordonn�es de la porte voisine
    public Door connectedDoor;// R�f�rence � la porte voisine

    /// <summary>
    /// Appel� lorsque le script est initialis� dans la sc�ne.
    /// </summary>
    private void Start()
    {
        teleportationManager = FindObjectOfType<TeleportationManager>();

        if (teleportationManager == null)
        {
            Debug.LogError("Error 701: TeleportationManager not found in the scene");
        }

        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(orientation))
        {
            InitializeDoor(id, x, y, orientation, isLocked, isLockedByBattle, isBossDoor);
        }
        else
        {
            Debug.LogWarning("Warning 308: Door not properly initialized. Check id and orientation");
        }
    }

    /// <summary>
    /// Initialise les propri�t�s de la porte.
    /// </summary>
    /// <param name="doorId"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="doorOrientation"></param>
    /// <param name="locked"></param>
    /// <param name="lockedByBattle"></param>
    /// <param name="bossDoor"></param>
    public void InitializeDoor(string doorId, float posX, float posY, string doorOrientation, bool locked, bool lockedByBattle, bool bossDoor)
    {
        id = doorId;
        x = posX;
        y = posY;
        orientation = doorOrientation;
        isLocked = locked;
        isLockedByBattle = lockedByBattle;
        isBossDoor = bossDoor;

        //Debug.Log($"Porte {id} initialis�e avec les coordonn�es ({x}, {y}), orientation {orientation}");
    }
    
    /// <summary>
    /// D�tecte lorsqu'un objet entre en collision avec la porte.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Le joueur a touch� la porte.");

            if (!isLocked && !isLockedByBattle)
            {
                if (connectedDoor != null)
                {
                    teleportationManager.TeleportPlayer(collision.gameObject, connectedDoor);
                }
                else
                {
                    Debug.LogWarning("Warning 309: No door connected is instantiated for this door");
                }
            }
            //else
            //{
            //    Debug.Log("La porte est verrouill�e.");
            //}
        }
    }

    /// <summary>
    /// Met � jour le sprite de la porte en fonction de son �tat.
    /// </summary>
    /// <param name="isOpen"></param>
    private void UpdateSprite(bool isOpen)
    {
        int orientationIndex = GetOrientationIndex(orientation);
        if (orientationIndex != -1)
        {
            if (isLocked)
            {
                spriteRenderer.sprite = spriteLock[orientationIndex];
            }
            else if (isBossDoor)
            {
                spriteRenderer.sprite = isOpen ? spriteBossOpen[orientationIndex] : spriteBossClose[orientationIndex];
            }
            else
            {
                spriteRenderer.sprite = isOpen ? spriteOpen[orientationIndex] : spriteClose[orientationIndex];
            }
        }
        else
        {
            Debug.Log($"ERROR 303 ({id}): La porte ne peut pas �tre mise � jour correctement");
        }
    }

    /// <summary>
    /// Retourne l'index correspondant � l'orientation de la porte.
    /// </summary>
    /// <param name="orientation"></param>
    private int GetOrientationIndex(string orientation)
    {
        switch (orientation)
        {
            case "N": return 0;
            case "S": return 1;
            case "E": return 2;
            case "W": return 3;
            default: return -1;
        }
    }

    /// <summary>
    /// Appel� lorsque le script est r�veill� dans la sc�ne.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// D�finit l'�tat de la porte comme ouverte.
    /// </summary>
    public void SetOpenDoor(bool isOpen)
    {
        if (!isLocked && CanBeOpen())
        {
            UpdateSprite(isOpen);
        }
    }

    /// <summary>
    /// V�rifie si la porte peut �tre ouverte.
    /// PROCEDURE GERE PAR LE PARTYMANAGER
    /// </summary>
    public bool CanBeOpen()
    {
        return true;
    }

    /// <summary>
    /// D�finit l'�tat de la porte comme ferm�e.
    /// </summary>
    public void SetCloseDoor()
    {
        UpdateSprite(false);
    }

    /// <summary>
    /// D�finit si la porte est verrouill�e.
    /// </summary>
    public void SetLocked(bool locked)
    {
        isLocked = locked;
        UpdateSprite(false);
    }

    /// <summary>
    /// V�rifie si la porte est verrouill�e.
    /// </summary>
    public bool DoorIsLocked()
    {
        return isLocked;
    }
}
