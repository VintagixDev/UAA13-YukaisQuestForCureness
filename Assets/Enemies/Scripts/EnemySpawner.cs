using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Préfabriqués")]
    [SerializeField, Tooltip("List d'ennemis")]
    private List<GameObject> enemies;
    [SerializeField, Tooltip("Prefab du boss")] 
    private GameObject bossPrefab;

    [Header("Attributs")]
    [SerializeField, Tooltip("ID de la pieces du spawn")]
    private int _roomID;
    [SerializeField, Tooltip("Position en x & y")]
    private Vector2 _position;
    [SerializeField, Tooltip("Est un spawner de boss")]
    private bool _isBossSpawner;

    private BattleManager BATTLE; // script : bataille

    public int RoomID
    {
        get { return _roomID; }
        set { _roomID = value; }
    }

    public void SpawnRandomEnemy()
    {
        GameObject Battle = GameObject.Find("BattleManager"); // Récupère le script de bataille
        if (Battle != null)
        {
            BATTLE = Battle.GetComponent<BattleManager>();
        } else
        {
            Debug.LogWarning("No Battle script available");
            return;
        }

        if (_isBossSpawner)
        {
            // Vérification
            if (bossPrefab == null)
            {
                Debug.LogWarning("Boss préfab is null or empty");
                return;
            }

            // Instantiate 
            GameObject instantiatedBoss = Instantiate(bossPrefab, transform.position, Quaternion.identity);

            // Assigner roomID au Boss uniquement
            if (instantiatedBoss.TryGetComponent(out Boss boss))
            {
                boss.ChangeRoomId(_roomID);  // Changer le RoomId dans le boss
            }

            // Initialiser le rendu en order in layer 2
            instantiatedBoss.GetComponent<SpriteRenderer>().sortingOrder = 2;

            // Ajout de 1 à la liste d'ennemis en vie
            BATTLE.AddEnemiesCount();

        } else
        {
            // Vérification
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

            // Ajout de 1 à la liste d'ennemis en vie
            BATTLE.AddEnemiesCount();
        }
    }
}
