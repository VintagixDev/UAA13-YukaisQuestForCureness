using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
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

    public void Start()
    {
        if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(orientation))
        {
            InitializeDoor(id, x, y, orientation, isLocked, isLockedByBattle, isBossDoor);
        }
        else
        {
            Debug.LogWarning("Door not properly initialized. Check id and orientation.");
        }
    }
    public void InitializeDoor(string doorId, float posX, float posY, string doorOrientation, bool locked, bool lockedByBattle, bool bossDoor)
    {
        // Initialisation des paramètres de la porte lors de instanciation
        id = doorId;
        x = posX;
        y = posY;
        orientation = doorOrientation;
        isLocked = locked;
        isLockedByBattle = lockedByBattle;
        isBossDoor = bossDoor;

        // Mettre à jour les sprites de la porte
        //UpdateSprite(false);

        Debug.Log($"Porte {id} initialisée avec les coordonnées ({x}, {y}), orientation {orientation}");
    }
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            // Vérification des conditions de passage
            if (!isLockedByBattle && !isLocked)
            {
                TeleportPlayer(collision.gameObject);
            }
            else
            {
                Debug.Log("Porte verrouillée");
            }
        }
    }
    private void TeleportPlayer(GameObject player)
    {
        // Téléportation directe avec les coordonnées de destination
        player.transform.position = new Vector2(x, y);

        // Mise à jour de la pièce actuelle du joueur
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.currentRoom = id; 
            Debug.Log($"Le joueur est maintenant dans la pièce {id}.");
        }

        // Log de téléportation avec l'ID de la porte
        Debug.Log($"Téléportation vers les coordonnées : ({x}, {y}) via la porte {id}");
    }

    // Méthodes pour ouvrir ou fermer la porte (en fonction de l'état de verrouillage et de l'orientation)
    public void SetOpenDoor(bool isOpen)
    {
        if (!isLocked && CanBeOpen())
        {
            UpdateSprite(isOpen);
        }
    }
    public bool CanBeOpen()
    {
        return true;
    }
    public void SetCloseDoor()
    {
        UpdateSprite(false);
    }
    public void SetLocked(bool locked)
    {
        isLocked = locked;
        UpdateSprite(false);
    }
    public bool DoorIsLocked()
    {
        return isLocked;
    }
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
            Debug.Log($"ERROR 303 ({id}): La porte ne peut pas être mise à jour correctement");
        }
    }
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
}
