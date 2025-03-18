using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] public List<GameObject> enemies;
    public int roomId = -1;

    public void SpawnRandomEnemy()
    {
        int nb = Random.Range(0, enemies.Count);
        GameObject enemy = enemies.ElementAt(nb);
        GameObject instanciatedEnemy = Instantiate(enemy);
        instanciatedEnemy.GetComponent<SpriteRenderer>().sortingOrder = 2;

    }

    public void SetRoomID(int id)
    {
        roomId = id;
    }
}
