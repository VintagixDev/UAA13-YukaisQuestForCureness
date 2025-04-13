using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        GameObject instanciatedEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        instanciatedEnemy.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
