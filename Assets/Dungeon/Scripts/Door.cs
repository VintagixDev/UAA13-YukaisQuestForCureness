using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Other")]
    [Tooltip("Script de supervision de partie")]
    [SerializeField] private GameSupervisor MASTER;
    [Tooltip("Script qui gère les déplacement du joueur")]
    [SerializeField] private TeleportationManager TELEPORTATION;
    [Tooltip("Script qui contient et gère les statistiques du jeu")]
    [SerializeField] private GameStat GAMESTAT;

    [Header("ID")]
    [Tooltip("Identifiant de la porte")]
    [SerializeField] private int _doorId;
    public int DoorID { get { return _doorId; } set { _doorId = value; } }

    [Tooltip("Identifiant de la pièces")]
    [SerializeField] private int _roomId;
    public int RoomID { get { return _roomId; } set { _roomId = value; } }

    [Header("Position/Coordinate")]
    [Tooltip("N : Nord\n" +
        "S : Sud\n" +
        "E : Est\n" +
        "W : Ouest\n")]
    [SerializeField] private string _orientation;
    public string Orientation { get { return _orientation; } set { _orientation = value; } }
    [Tooltip("")]
    [SerializeField] private float _x;
    [Tooltip("")]
    [SerializeField] private float _y;

    [Header("Sprites")]
    [Tooltip("")]
    [SerializeField] private Sprite _open;
    [Tooltip("")]
    [SerializeField] private Sprite _close;
    [Tooltip("")]
    [SerializeField] private Sprite _bossOpen;
    [Tooltip("")]
    [SerializeField] private Sprite _bossClose;
    [Tooltip("")]
    [SerializeField] private SpriteRenderer _spriteRenderer;

    [Header("Box Collider 2D")]
    [Tooltip("")]
    [SerializeField] private BoxCollider2D _boxCollider;

    [Header("State")]
    private bool _isLockedByBattle; // Si vrai, la bataille est en cours et le sprite est différent
    private bool _isBossDoor; // Si vrai, c'est une porte de boss et son sprite est différent

    [Header("Link")]
    public Vector2Int connectedDoorPosition;
    public Door connectedDoor;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        MASTER = FindObjectOfType<GameSupervisor>();
        TELEPORTATION = FindObjectOfType<TeleportationManager>();
        GAMESTAT = FindObjectOfType<GameStat>();

        if (TELEPORTATION == null)
        {
            Debug.LogError("Error 701: TeleportationManager not found in the scene");
        }

        UpdateDoorSprite();
    }

    public void InitializeDoor(int doorIdCount, int roomId, float posX, float posY, string doorOrientation,
        bool locked, bool lockedByBattle, bool bossDoor)
    {
        _doorId = doorIdCount;
        _roomId = roomId;
        _x = posX;
        _y = posY;
        _orientation = doorOrientation;
        _isLockedByBattle = lockedByBattle;
        _isBossDoor = bossDoor;

        UpdateDoorSprite();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || _isLockedByBattle)
            return;

        if (connectedDoor == null)
        {
            Debug.LogWarning("Warning 309: No connected door is instantiated for this door.");
            return;
        }

        TELEPORTATION.TeleportPlayer(collision.gameObject, connectedDoor, _roomId);
    }

    private void UpdateDoorSprite()
    {
        int getOrientationINT = GetOrientationIndex();
        if (getOrientationINT != -1)
        {
            switch (getOrientationINT)
            {
                case 0: transform.rotation = Quaternion.Euler(0f, 0f, 0f); break;      // N
                case 1: transform.rotation = Quaternion.Euler(0f, 0f, 180f); break;    // S
                case 2: transform.rotation = Quaternion.Euler(0f, 0f, -90f); break;    // E
                case 3: transform.rotation = Quaternion.Euler(0f, 0f, 90f); break;     // W
            }
        }

        if (_isBossDoor)
        {
            _spriteRenderer.sprite = _isLockedByBattle ? _bossClose : _bossOpen;
        }
        else
        {
            _spriteRenderer.sprite = _isLockedByBattle ? _close : _open;
        }
    }

    public void CloseUnClose(bool lockedByBattle)
    {
        _isLockedByBattle = lockedByBattle;
        _spriteRenderer.sprite = lockedByBattle ? _close : _open;
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
            case "N": return 0;
            case "S": return 1;
            case "E": return 2;
            case "W": return 3;
            default: return -1;
        }
    }

    // accessors
    public int GetDoorId() => _doorId;
    public int GetRoomId() => _roomId;
    public string GetOrientation() => _orientation;
    public float GetX() => _x;
    public float GetY() => _y;
    public bool IsLockedByBattle() => _isLockedByBattle;
    public bool IsBossDoor() => _isBossDoor;
}
