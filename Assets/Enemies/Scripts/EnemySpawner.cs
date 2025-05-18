using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("")]
    [Tooltip("List d'ennemis")]
    public List<GameObject> enemies;
    [Tooltip("ID de la pieces du spawn")]
    [SerializeField] private int _roomID;
    [Tooltip("Position en x & y")]
    [SerializeField] private Vector2 _position;

    public int RoomID
    {
        get { return _roomID; }
        set { _roomID = value; }
    }

    public void SpawnRandomEnemy()
    {
        if (enemies == null || enemies.Count == 0)
        {
            Debug.LogWarning("Enemy list is empty or null.");
            return;
        }

        int nb = Random.Range(0, enemies.Count);
        GameObject enemy = enemies[nb];

        // Instantiate the enemy
        GameObject instantiatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);

        // Assigner roomID au Snail uniquement (en fonction de l'objet)
        if (instantiatedEnemy.TryGetComponent(out Snail snail))
        {
            snail.ChangeRoomId(_roomID);  // Changer le RoomId dans Snail
        }

        // Initialiser le rendu et les autres comportements spécifiques
        instantiatedEnemy.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
