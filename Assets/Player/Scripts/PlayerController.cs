using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Tooltip("Vitesse de d�placement de base du joueur")]
    private float _speed = 4.5f;
    [SerializeField, Tooltip("Facteur de diminution progressive de la vitesse (glissance)")]
    private float _speedDecrease = 1.025f;
    [SerializeField, Tooltip("Permet de d�sactiver ou activer le d�placement du joueur")]
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
    [SerializeField, Tooltip("Prefab du projectile tir� par le joueur")]
    private GameObject _bulletPrefabUp;
    [SerializeField, Tooltip("Prefab du projectile tir� par le joueur")]
    private GameObject _bulletPrefabDown;
    [SerializeField, Tooltip("Prefab du projectile tir� par le joueur")]
    private GameObject _bulletPrefabRight;
    [SerializeField, Tooltip("Prefab du projectile tir� par le joueur")]
    private GameObject _bulletPrefabLeft;
    [SerializeField, Tooltip("Nombre de tirs possibles par seconde")]
    private float _bulletRate = 2f;
    [SerializeField, Tooltip("Vitesse du projectile tir�")]
    private float _projectileSpeed = 8f;
    [SerializeField, Tooltip("Dur�e avant destruction du projectile (port�e)")]
    private float _projectileLifetime = 2f;
    [SerializeField, Tooltip("Taille du projectile (scale)")]
    private float _projectileSize = 1f;
    private bool _canShoot = true;

    [Header("Invincibility")]
    [SerializeField, Tooltip("Dur�e en frames de l'invincibilit� apr�s avoir subi des d�g�ts")]
    private int _iFrames = 160;
    private int _cdTime;

    [Header("References")]
    [SerializeField, Tooltip("Composant des statistiques du joueur")]
     private PlayerStats _stats;
    [SerializeField, Tooltip("Interface utilisateur des statistiques")]
     private StatsUI _statsUI;
    [SerializeField, Tooltip("R�f�rence au script d'animation du joueur")]
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
    /// G�re les entr�es clavier pour d�placer le joueur avec une inertie simul�e,
    /// met � jour l'animation du joueur en fonction de la direction.
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
    /// G�re les entr�es clavier pour tirer dans quatre directions (haut, bas, gauche, droite).
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
    /// Instancie un projectile depuis un point de spawn donn�, le propulse dans une direction sp�cifique,
    /// et applique les param�tres d�finis (vitesse, dur�e de vie, taille).
    /// </summary>
    /// <param name="spawnPoint">Transform du point d'apparition du projectile.</param>
    /// <param name="direction">Direction dans laquelle le projectile sera tir�.</param>
    private void Shoot(Transform spawnPoint, Vector2 direction)
    {
        _canShoot = false;
        StartCoroutine(ResetShootCooldown());

        GameObject bulletPrefab = null;

        if (direction == Vector2.up) // haut
            bulletPrefab = _bulletPrefabUp;
        else if (direction == Vector2.down) // bas
            bulletPrefab = _bulletPrefabDown;
        else if (direction == Vector2.left) // gauche
            bulletPrefab = _bulletPrefabLeft;
        else if (direction == Vector2.right) // droite
            bulletPrefab = _bulletPrefabRight;

        if (bulletPrefab == null)
        {
            Debug.LogError("bulletPrefab est null");
            return; // s�curit� en cas de mauvaise manip
        }

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * _projectileSpeed;
        bullet.GetComponent<PlayerProjectileBehaviour>().TimeToDeath = _projectileLifetime;
        bullet.transform.localScale *= _projectileSize;
    }

    /// <summary>
    /// Coroutine qui r�active la capacit� de tirer apr�s un d�lai d�fini par le taux de tir (_bulletRate).
    /// </summary>
    /// <returns>IEnumerator pour coroutine.</returns>
    private IEnumerator ResetShootCooldown()
    {
        yield return new WaitForSeconds(1f / _bulletRate);
        _canShoot = true;
    }

    /// <summary>
    /// G�re le compteur d'invincibilit� apr�s que le joueur ait pris des d�g�ts.
    /// R�duit le cooldown jusqu'� r�activation de la vuln�rabilit�.
    /// </summary>
    private void HandleInvincibilityFrames()
    {
        if (_cdTime > 0)
        {
            _cdTime--;
        }
    }

    /// <summary>
    /// Applique des d�g�ts au joueur si ce dernier est vuln�rable,
    /// met � jour l'affichage de la vie, et d�clenche la sc�ne de mort si les PV tombent � z�ro.
    /// </summary>
    /// <param name="damage">Quantit� de d�g�ts � infliger.</param>
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
    /// G�re les collisions d�clench�es par les colliders du joueur.
    /// D�l�gue la logique selon le collider impliqu�.
    /// </summary>
    /// <param name="collision">Collider d�clencheur.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (_hitboxCollider.IsTouching(collision))
        {
            HandleHitboxCollision(collision.gameObject);
        }
    }

    /// <summary>
    /// G�re les interactions avec des objets affectant la hitbox du joueur, comme projectiles ou objets ramassables.
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

         
        }
    }

    /// <summary>
    /// Applique les effets des am�liorations du joueur.
    /// Ex�cute les actions des objets de type Upgrade.
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
