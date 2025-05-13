using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] public List<GameObject> enemies;
    [SerializeField] private int _roomID;
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
            snail.roomID = _roomID;  // Assigner l'ID de la salle au Snail
        }

        // Initialiser le rendu et les autres comportements spécifiques
        instantiatedEnemy.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
