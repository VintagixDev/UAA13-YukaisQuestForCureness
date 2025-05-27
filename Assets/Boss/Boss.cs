using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{

    public int bossId;
    public string bossName;

    public int bossMaxHP;
    public int bossCurrentHP;
    public float bossMovementSpeed;
    public bool bossIsRange;

    [Header("Movements")]
    public bool isStunned;

    [Header("Ranged")]
    public float enemyProjectileSize = 1;
    public float enemyProjectileReach = 1;
    public float enemyProjectileSpeed = 1;
    public int enemyAttackSpeed = 60; // Nombre de frame
    public int cdAttackSpeed;

    public Slider healthBar;
    public GameObject bossUI;

    public int _roomID;

    [Header("Scripts")]
    [SerializeField]
    private BattleManager BATTLE; 
    [SerializeField] 
    private GameSupervisor MASTER;

    [Header("Drop")]
    [SerializeField, Tooltip("Pierre de téléportation entre les niveaux")]
    private GameObject _stone;
    [SerializeField, Tooltip("Tableaux des drops (upgrades)")]
    private GameObject[] _drops;


    public void Awake()
    {
        bossCurrentHP = bossMaxHP;

        // Récupération des composants
        MASTER = GameObject.FindGameObjectWithTag("GameSupervisor").GetComponent<GameSupervisor>();
        BATTLE = GameObject.FindGameObjectWithTag("BattleManager").GetComponent<BattleManager>();

        if (bossUI == null)
        {
            bossUI = MASTER.bossUI;
            bossUI.SetActive(true);
            healthBar = bossUI.transform.GetChild(0).GetComponent<Slider>();

            healthBar.value = healthBar.maxValue;
            healthBar.gameObject.SetActive(true);
        }

    }

    // Update is called once per frame
    private void Update()
    {
        //if(bossUI == null)
        //{
        //    bossUI = MASTER.bossUI;
        //    bossUI.SetActive(true);
        //    healthBar = bossUI.transform.GetChild(0).GetComponent<Slider>();

        //    healthBar.value = healthBar.maxValue;
        //    healthBar.gameObject.SetActive(true);
        //}
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(50);
        }


        if (isStunned) return;
        if (bossIsRange)
        {
            cdAttackSpeed--;
            if (cdAttackSpeed == 0)
            {
                cdAttackSpeed = enemyAttackSpeed;
                EnemyRangeAttack();
            }

        }
    }
    private void Die()
    {
        BATTLE.RemoveEnemiesCount();
        BATTLE.FinishBattleMethod(); 

        Instantiate(_stone, transform.position, Quaternion.identity);

        if (_drops != null && _drops.Length > 0)
        {
            int randomIndex = Random.Range(0, _drops.Length);
            Instantiate(_drops[randomIndex], new Vector3(0, 5, 0), Quaternion.identity);
        }

        Destroy(this.gameObject.GetComponent<Rigidbody2D>());
        Destroy(this.gameObject.GetComponent<BoxCollider2D>());
        Destroy(this.gameObject.GetComponent<SpriteRenderer>());
           
        Destroy(gameObject, 0.1f);
    }

    public virtual void EnemyRangeAttack() { }

    public void TakeDamage(int damage)
    {
        bossCurrentHP -= damage;
        if(bossCurrentHP <= 0)
        {
            // WIN
            healthBar.value = 0;
            bossUI.SetActive(false);
            Die();
        }
        healthBar.value = 100 * bossCurrentHP / bossMaxHP;
    }

    public void ChangeRoomId(int newRoomId)
    {
        _roomID = newRoomId;
        //Debug.Log($"RoomId changé pour {newRoomId}");
    }

}
