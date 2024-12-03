using UnityEngine;
using System.Collections.Generic;

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
    private bool isLocked = false;
    private bool isBossDoor = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetLocalState(string typeOfDoor, string orientationFromManager, string idFromManager)
    {
        id = idFromManager;
        orientation = orientationFromManager;
        isBossDoor = typeOfDoor == "Boss";

        int orientationIndex = GetOrientationIndex(orientation);
        if (orientationIndex != -1)
        {
            UpdateSprite(false);
            Debug.Log($"({id}): The Door was correctly instantiated");
        }
        else
        {
            Debug.Log($"ERROR 301 ({id}): The Door can't be properly set");
        }
    }

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
        } else
        {
            Debug.Log($"ERROR 303 ({id}): The Door can't be properly updated");
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