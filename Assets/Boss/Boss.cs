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

    [Header("Room")]
    public int _roomID;
    private BattleManager BATTLE; // script : bataille
    public GameSupervisor gameSupervisor;




    void Start()
    {
        
        bossCurrentHP = bossMaxHP;
        GameObject Battle = GameObject.Find("BattleManager"); // Récupère le script de bataille
        if (Battle != null)
        {
            BATTLE = Battle.GetComponent<BattleManager>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if(bossUI == null)
        {
            gameSupervisor = GameObject.FindGameObjectWithTag("GameSupervisor").GetComponent<GameSupervisor>();
            bossUI = gameSupervisor.bossUI;
            bossUI.SetActive(true);
            healthBar = bossUI.transform.GetChild(0).GetComponent<Slider>();

            healthBar.value = healthBar.maxValue;
            healthBar.gameObject.SetActive(true);
        }
        
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
        BATTLE._remainingEnemies--;
        BATTLE.FinishBattleMethod(); // Logique pour terminer la bataille
        Destroy(gameObject); // Destruction de l'objet
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
