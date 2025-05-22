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
    [SerializeField, Tooltip("Prefab du boss")] public GameObject bossPrefab;

    public int RoomID
    {
        get { return _roomID; }
        set { _roomID = value; }
    }

    public void SpawnRandomEnemy()
    {

        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        if (rooms.Length == 0) return;


        foreach (GameObject room in rooms)
        {
            Room roomScript = room.GetComponent<Room>();
            if (roomScript.RoomID == _roomID && roomScript.isBossRoom)
            {
                GameObject bossSpawner = GameObject.FindGameObjectWithTag("EnemySpawner");
                GameObject boss = Instantiate(bossPrefab, bossSpawner.transform.position, Quaternion.identity);
                Boss bossScript = boss.GetComponent<Boss>();
                bossScript.ChangeRoomId(_roomID);
                boss.GetComponent<SpriteRenderer>().sortingOrder = 2;
                return;
            }
        }
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
