using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Other")]
    [SerializeField] private GameSupervisor MASTER;
    [SerializeField] private TeleportationManager TELEPORTATION;
    [SerializeField] private GameStat GAMESTAT;

    [Header("ID")]
    [SerializeField] public int _doorId;
    [SerializeField] public int _roomId;

    [Header("Position/Coordinate")]
    [SerializeField] public string _orientation;
    [SerializeField] public float x;
    [SerializeField] public float y;

    [Header("Sprites")]
    [SerializeField] public Sprite Open;
    [SerializeField] public Sprite Close;
    [SerializeField] public Sprite BossOpen;
    [SerializeField] public Sprite BossClose;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Box Collider 2D")]
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("State")]
    public bool _isLockedByBattle;
    public bool _isBossDoor;


    [Header("Link")]
    public Vector2Int connectedDoorPosition;
    public Door connectedDoor;

    private void Start()
    {
        MASTER = FindObjectOfType<GameSupervisor>();
        TELEPORTATION = FindObjectOfType<TeleportationManager>();

        if (TELEPORTATION == null)
        {
            Debug.LogError("Error 701: TeleportationManager not found in the scene");
        }

        UpdateDoorSprite();
    }

    /// <summary>
    /// Initialise les propriétés de la porte.
    /// </summary>
    /// <param name="doorIdCount"></param>
    /// <param name="roomId"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <param name="doorOrientation"></param>
    /// <param name="locked"></param>
    /// <param name="lockedByBattle"></param>
    /// <param name="bossDoor"></param>
    public void InitializeDoor(int doorIdCount, int roomId, float posX, float posY, string doorOrientation,
        bool locked, bool lockedByBattle, bool bossDoor)
    {
        _doorId = doorIdCount;
        _roomId = roomId;
        x = posX;
        y = posY;
        _orientation = doorOrientation;
        _isLockedByBattle = lockedByBattle;
        _isBossDoor = bossDoor;

        // Sélectionner le bon sprite
        UpdateDoorSprite();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !_isLockedByBattle)
        {
            if (connectedDoor != null)
            {
                TELEPORTATION.TeleportPlayer(collision.gameObject, connectedDoor, _roomId);
            }
            else
            {
                Debug.LogWarning("Warning 309: No door connected is instantiated for this door");
            }
        }
    }

    private void UpdateDoorSprite()
    {
        int getOrientationINT = GetOrientationIndex();
        if (getOrientationINT != -1)
        {
            if (getOrientationINT == 0)
            {
                // Nord / Orientation normale
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            } else if (getOrientationINT == 1)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 180f);
            } else if (getOrientationINT == 2) 
            {
                transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            } else if (getOrientationINT == 3)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }

        if (_isBossDoor)
        {
            if (_isLockedByBattle)
            {
                spriteRenderer.sprite = BossClose; // Porte de boss fermée
            }
            else
            {
                spriteRenderer.sprite = BossOpen; // Porte de boss ouverte
            }
        }
        else
        {
            if (_isLockedByBattle)
            {
                spriteRenderer.sprite = Close; // Porte normale fermée
            }
            else
            {
                spriteRenderer.sprite = Open; // Porte normale ouverte
            }
        }
    }


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void CloseUnClose(bool lockedByBattle)
    {
        _isLockedByBattle = lockedByBattle;
        if(lockedByBattle) 
        {
            spriteRenderer.sprite = Close;
        } else
        {
            spriteRenderer.sprite = Open;
        }
    }
    public void SetDoorState(bool lockedByBattle, bool bossDoor)
    {
        _isLockedByBattle = lockedByBattle;
        _isBossDoor = bossDoor;
    }

    private int GetOrientationIndex()
    {
        switch (_orientation)
        {
            case "N": return 0;     // Nord (Valeur par défaut)
            case "S": return 1;     // Est
            case "E": return 2;     // Sud
            case "W": return 3;     // Ouest
            default: return -1;     // Valeur par défaut 
        }
    }
}