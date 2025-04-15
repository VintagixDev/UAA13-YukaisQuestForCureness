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
     
    void Start()
    {
        bossCurrentHP = bossMaxHP;
        healthBar.value = healthBar.maxValue;
        healthBar.gameObject.SetActive(true);
    }

    // Update is called once per frame
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(50);
        }


        if (isStunned) return;
        Movements();
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
    public virtual void Movements()
    {

    }

    public virtual void EnemyRangeAttack() { }

    public void TakeDamage(int damage)
    {
        bossCurrentHP -= damage;
        if(bossCurrentHP <= 0)
        {
            // WIN
            healthBar.value = 0;
            Destroy(gameObject);
            healthBar.gameObject.SetActive(false);
        }
        healthBar.value = 100 * bossCurrentHP / bossMaxHP;
    }

}
