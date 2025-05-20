using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Vitesse de déplacement de base du joueur")]
    private float _speed = 4.5f;
    [SerializeField, Tooltip("Facteur de diminution progressive de la vitesse (glissance)")]
    private float _speedDecrease = 1.025f;
    [SerializeField, Tooltip("Permet de désactiver ou activer le déplacement du joueur")]
    private bool _canMove = true;
    private float _horizontalVelocity = 0f;
    private float _verticalVelocity = 0f;

    [Header("Shooting")]
    [SerializeField, Tooltip("Point d'apparition des projectiles vers le haut")]
    private Transform _upSpawn;
    [SerializeField, Tooltip("Point d'apparition des projectiles vers le bas")]
    private Transform _downSpawn;
    [SerializeField, Tooltip("Point d'apparition des projectiles vers la gauche")]
    private Transform _leftSpawn;
    [SerializeField, Tooltip("Point d'apparition des projectiles vers la droite")]
    private Transform _rightSpawn;

    [Header("Bullet settings")]
    [SerializeField, Tooltip("Prefab du projectile tiré par le joueur")]
    private GameObject _bulletPrefabUp;
    [SerializeField, Tooltip("Prefab du projectile tiré par le joueur")]
    private GameObject _bulletPrefabDown;
    [SerializeField, Tooltip("Prefab du projectile tiré par le joueur")]
    private GameObject _bulletPrefabRight;
    [SerializeField, Tooltip("Prefab du projectile tiré par le joueur")]
    private GameObject _bulletPrefabLeft;
    [SerializeField, Tooltip("Nombre de tirs possibles par seconde")]
    private float _bulletRate = 2f;
    [SerializeField, Tooltip("Vitesse du projectile tiré")]
    private float _projectileSpeed = 8f;
    [SerializeField, Tooltip("Durée avant destruction du projectile (portée)")]
    private float _projectileLifetime = 2f;
    [SerializeField, Tooltip("Taille du projectile (scale)")]
    private float _projectileSize = 1f;
    private bool _canShoot = true;

    [Header("Invincibility")]
    [SerializeField, Tooltip("Durée en frames de l'invincibilité après avoir subi des dégâts")]
    private int _iFrames = 160;
    private int _cdTime;

    [Header("References")]
    [SerializeField, Tooltip("Composant des statistiques du joueur")]
     private PlayerStats _stats;
    [SerializeField, Tooltip("Interface utilisateur des statistiques")]
     private StatsUI _statsUI;
    [SerializeField, Tooltip("Référence au script d'animation du joueur")]
    private PlayerAnimation _playerAnimation;
    [SerializeField, Tooltip("Collider des pieds du joueur")]
    private BoxCollider2D _footCollider;
    [SerializeField, Tooltip("Collider de la hitbox du joueur")]
    private BoxCollider2D _hitboxCollider;

    private void Start()
    {
        _cdTime = _iFrames;

        if (_stats == null)
        {
            _stats = GetComponent<PlayerStats>();
        }

        if (_footCollider != null)
        {
            _footCollider.isTrigger = false;
        }

        if (_hitboxCollider != null)
        {
            _hitboxCollider.isTrigger = true;
        }

        _statsUI = GameObject.Find("HUD").GetComponent<StatsUI>();
    }

    private void Update()
    {
        if (_canMove)
        {
            HandleMovement();
        }

        if (_canShoot)
        {
            HandleShooting();
        }

        HandleInvincibilityFrames();
        ApplyUpgrades();
    }

    /// <summary>
    /// Gère les entrées clavier pour déplacer le joueur avec une inertie simulée,
    /// met à jour l'animation du joueur en fonction de la direction.
    /// </summary>
    private void HandleMovement()
    {
        // Horizontal movement
        if (Input.GetKey(KeyCode.A))
        {
            if (_horizontalVelocity > -1f)
            {
                _horizontalVelocity -= 0.1f;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (_horizontalVelocity < 1f)
            {
                _horizontalVelocity += 0.1f;
            }
        }
        else
        {
            if (_horizontalVelocity < 0f)
            {
                _horizontalVelocity = _horizontalVelocity / _speedDecrease;
                if (_horizontalVelocity > -0.1f)
                {
                    _horizontalVelocity = 0f;
                }
            }
            else if (_horizontalVelocity > 0f)
            {
                _horizontalVelocity = _horizontalVelocity / _speedDecrease;
                if (_horizontalVelocity < 0.1f)
                {
                    _horizontalVelocity = 0f;
                }
            }
        }

        // Vertical movement
        if (Input.GetKey(KeyCode.W))
        {
            if (_verticalVelocity < 1f)
            {
                _verticalVelocity += 0.1f;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (_verticalVelocity > -1f)
            {
                _verticalVelocity -= 0.1f;
            }
        }
        else
        {
            if (_verticalVelocity < 0f)
            {
                _verticalVelocity = _verticalVelocity / _speedDecrease;
                if (_verticalVelocity > -0.1f)
                {
                    _verticalVelocity = 0f;
                }
            }
            else if (_verticalVelocity > 0f)
            {
                _verticalVelocity = _verticalVelocity / _speedDecrease;
                if (_verticalVelocity < 0.1f)
                {
                    _verticalVelocity = 0f;
                }
            }
        }

        Vector2 direction = new Vector2(_horizontalVelocity, _verticalVelocity);
        transform.Translate(direction * _speed * _stats.playerMoveSpeed * Time.deltaTime);
        _playerAnimation.UpdateAnimation(_horizontalVelocity, _verticalVelocity);
    }

    /// <summary>
    /// Gère les entrées clavier pour tirer dans quatre directions (haut, bas, gauche, droite).
    /// </summary>
    private void HandleShooting()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Shoot(_leftSpawn, Vector2.left);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            Shoot(_rightSpawn, Vector2.right);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            Shoot(_upSpawn, Vector2.up);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            Shoot(_downSpawn, Vector2.down);
        }
    }

    /// <summary>
    /// Instancie un projectile depuis un point de spawn donné, le propulse dans une direction spécifique,
    /// et applique les paramètres définis (vitesse, durée de vie, taille).
    /// </summary>
    /// <param name="spawnPoint">Transform du point d'apparition du projectile.</param>
    /// <param name="direction">Direction dans laquelle le projectile sera tiré.</param>
    private void Shoot(Transform spawnPoint, Vector2 direction)
    {
        _canShoot = false;
        StartCoroutine(ResetShootCooldown());

        GameObject bulletPrefab = null;

        if (direction == Vector2.up) bulletPrefab = _bulletPrefabUp;
        else if (direction == Vector2.down) bulletPrefab = _bulletPrefabDown;
        else if (direction == Vector2.left) bulletPrefab = _bulletPrefabLeft;
        else if (direction == Vector2.right) bulletPrefab = _bulletPrefabRight;

        if (bulletPrefab == null) return; // sécurité

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * _projectileSpeed;
        bullet.GetComponent<BulletShoot>()._timeToDeath = _projectileLifetime;
        bullet.transform.localScale *= _projectileSize;
    }

    /// <summary>
    /// Coroutine qui réactive la capacité de tirer après un délai défini par le taux de tir (_bulletRate).
    /// </summary>
    /// <returns>IEnumerator pour coroutine.</returns>
    private IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(1f / _bulletRate);
        _canShoot = true;
    }

    /// <summary>
    /// Gère le compteur d'invincibilité après que le joueur ait pris des dégâts.
    /// Réduit le cooldown jusqu'à réactivation de la vulnérabilité.
    /// </summary>
    private void HandleInvincibilityFrames()
    {
        if (_cdTime > 0)
        {
            _cdTime--;
        }
    }

    /// <summary>
    /// Applique des dégâts au joueur si ce dernier est vulnérable,
    /// met à jour l'affichage de la vie, et déclenche la scène de mort si les PV tombent à zéro.
    /// </summary>
    /// <param name="damage">Quantité de dégâts à infliger.</param>
    public void DamagePlayer(int damage)
    {
        if (_cdTime <= 0)
        {
            _stats.playerHP -= damage;

            if (_stats.playerHP <= 0)
            {
                SceneManager.LoadScene("DeathScene");
            }

            _statsUI.updateDisplayHearts();
            _cdTime = _iFrames; 
        }
    }

    /// <summary>
    /// Gère les collisions déclenchées par les colliders du joueur.
    /// Délègue la logique selon le collider impliqué.
    /// </summary>
    /// <param name="collision">Collider déclencheur.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (_hitboxCollider.IsTouching(collision))
        {
            HandleHitboxCollision(collision.gameObject);
        }
    }

    /// <summary>
    /// Gère les interactions avec des objets affectant la hitbox du joueur, comme projectiles ou objets ramassables.
    /// </summary>
    /// <param name="obj">Objet entrant en collision avec la hitbox.</param>
    private void HandleHitboxCollision(GameObject obj)
    {
        switch (obj.tag)
        {
            case "FriendlyProjectile":
                return;

            case "Gold":
                Destroy(obj);
                _stats.playerGolds++;
                _statsUI.updateCollectableUI();
                break;

            case "Key":
                Destroy(obj);
                _stats.playerKeys++;
                _statsUI.updateCollectableUI();
                break;

            //case "EnemyProjectile":
                //Destroy(obj);
                //DamagePlayer(1);
                //break;
        }
    }

    /// <summary>
    /// Applique les effets des améliorations du joueur.
    /// Exécute les actions des objets de type Upgrade.
    /// </summary>
    private void ApplyUpgrades()
    {
        foreach (GameObject upgrade in _stats.playerUpgrades)
        {
            Upgrade upg = upgrade.GetComponent<Upgrade>();
            if (!upg.upgradeEffectOnce || !upg.upgradeHasBeenUsed)
            {
                upg.UpgradeAction();
            }
        }
    }

}
