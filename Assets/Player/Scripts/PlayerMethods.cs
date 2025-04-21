using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMethods : MonoBehaviour
{
    [Header("Invincibility")]
    [SerializeField] public int iFrames = 160;
    [SerializeField] private int cdTime;

    [Header("Scripts")]
    [SerializeField] public PlayerStats stats;
    [SerializeField] public StatsUI statsUI;

    [Header("Collider2D")]
    [SerializeField] public BoxCollider2D _foot;
    [SerializeField] public BoxCollider2D _hitBox;

    void Start()
    {
        cdTime = iFrames;

        if (stats == null)
        {
            stats = GetComponent<PlayerStats>();
        }

        // Vérifie que les colliders sont bien en trigger
        if (_foot != null) _foot.isTrigger = true;
        if (_hitBox != null) _hitBox.isTrigger = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            DamagePlayer(1);
        }

        if (cdTime < iFrames)
        {
            cdTime--;
            if (cdTime == 0)
            {
                cdTime = iFrames;
            }
        }

        // Upgrade effects
        foreach (GameObject upgrade in stats.playerUpgrades)
        {
            Upgrade upg = upgrade.GetComponent<Upgrade>();
            if (!upg.upgradeEffectOnce || !upg.upgradeHasBeenUsed)
            {
                upg.UpgradeAction();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;

        // Vérifie si c'est la hitbox ou les pieds qui ont déclenché la collision
        if (collision == _foot)
        {
            HandleFootCollision(obj);
        }
        else if (collision == _hitBox)
        {
            HandleHitboxCollision(obj);
        }
    }

    void HandleFootCollision(GameObject obj)
    {
        // Ici tu peux détecter les sols, pièges, plateformes spéciales, etc.
        if (obj.CompareTag("Trap"))
        {
            DamagePlayer(1);
        }
        else if (obj.CompareTag("SlipperyFloor"))
        {
            Debug.Log("Attention ça glisse !");
            // Modifier la friction ou autre ici
        }
    }

    void HandleHitboxCollision(GameObject obj)
    {
        // Gère les projectiles, ennemis, loot
        switch (obj.tag)
        {
            case "FriendlyProjectile":
                return;

            case "Gold":
                Destroy(obj);
                stats.playerGolds++;
                statsUI.updateCollectableUI();
                break;

            case "Key":
                Destroy(obj);
                stats.playerKeys++;
                statsUI.updateCollectableUI();
                break;

            case "EnemyProjectile":
                Destroy(obj);
                DamagePlayer(1);
                break;
        }
    }

    public void DamagePlayer(int damage)
    {
        if (cdTime == iFrames)
        {
            stats.playerHP -= damage;
            if (stats.playerHP <= 0)
            {
                SceneManager.LoadScene("DeathScene");
            }
            statsUI.updateDisplayHearts();
            cdTime--;
        }
    }
}
