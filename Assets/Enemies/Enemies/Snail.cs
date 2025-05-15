using UnityEngine;

public class Snail : MonoBehaviour
{
    [Header("Données")]
    [Tooltip("ID de la salle à laquelle appartient l'ennemi")]
    [SerializeField] private int _roomID;

    [Tooltip("Santé de l'ennemi")]
    [SerializeField] private int _health;

    [Tooltip("Vitesse de l'ennemi")]
    [SerializeField] private float _moveSpeed;

    [Header("Détection")]
    [Tooltip("Objet Joueur")]
    [SerializeField] private GameObject _player;
    [Tooltip("Transform du joueur pour référence")]
    [SerializeField] private Transform _playerTransform;

    [Header("Mouvement")]
    [Tooltip("Distance minimale à laquelle l'ennemi commence à fuir le joueur")]
    [SerializeField] private float _minDistance = 4f;
    [Tooltip("Distance maximale à laquelle l'ennemi commence à se rapprocher du joueur")]
    [SerializeField] private float _maxDistance = 7f;
    private bool _isMoving = false;

    [Header("Attaque")]
    [Tooltip("Préfabriqué du projectile que l'ennemi utilise pour attaquer")]
    [SerializeField] private GameObject _projectilePrefab;
    [Tooltip("Point de tir pour les projectiles")]
    [SerializeField] private Transform _shootPoint;
    [Tooltip("Plage d'attaque (distance à partir de laquelle l'ennemi peut tirer)")]
    [SerializeField] private float _attackRange = 6f;
    [Tooltip("Temps de recharge entre chaque tir")]
    [SerializeField] private float _attackCooldown = 2f;
    private float _lastAttackTime = 0f;

    // Variables pour l'animation
    private float _horizontalVelocity { get; set; }
    private float _verticalVelocity { get; set; }

    private EnemyScriptAnimation _enemyAnimation;

    void Start()
    {
        if (_player == null)
        {
            GameObject foundPlayer = GameObject.FindWithTag("Player");
            if (foundPlayer != null)
            {
                _player = foundPlayer;
            }
            else
            {
                Debug.LogError("Snail : Aucun joueur trouvé avec le tag 'Player'.");
                return;
            }
        }

        _playerTransform = _player.transform;
        _enemyAnimation = GetComponent<EnemyScriptAnimation>(); // Récupère le script d'animation
    }

    void Update()
    {
        if (_playerTransform == null) return;

        Move();

        // Appeler le gestionnaire d'animations
        if (_enemyAnimation != null)
        {
            _enemyAnimation.SetAnimation(_horizontalVelocity, _verticalVelocity);
        }

        // Gérer l'attaque
        TryAttack();
    }

    // Logique de déplacement (rendre la méthode publique pour implémenter l'interface)
    public void Move()
    {
        Vector3 direction = _playerTransform.position - transform.position;
        float distance = direction.magnitude;

        if (distance < _minDistance)
        {
            // Si trop proche, fuir le joueur
            direction = -direction.normalized;
            _horizontalVelocity = direction.x * _moveSpeed;
            _verticalVelocity = direction.y * _moveSpeed;
            _isMoving = true;
        }
        else if (distance > _maxDistance)
        {
            // Si trop éloigné, se rapprocher du joueur
            direction = direction.normalized;
            _horizontalVelocity = direction.x * _moveSpeed;
            _verticalVelocity = direction.y * _moveSpeed;
            _isMoving = true;
        }
        else
        {
            _horizontalVelocity = 0f;
            _verticalVelocity = 0f;
            _isMoving = false;
        }

        // Déplacement réel
        transform.position += direction * _moveSpeed * Time.deltaTime;
    }

    // Gérer l'attaque
    private void TryAttack()
    {
        float distance = Vector3.Distance(transform.position, _playerTransform.position);

        if (distance <= _attackRange && Time.time - _lastAttackTime >= _attackCooldown)
        {
            _lastAttackTime = Time.time;

            // Instancier le projectile
            GameObject proj = Instantiate(_projectilePrefab, _shootPoint.position, Quaternion.identity);

            // Direction du projectile vers le joueur
            Vector3 direction = (_playerTransform.position - _shootPoint.position).normalized;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();

            if (rb != null)
                rb.velocity = direction * 10f; // Vitesse du projectile
        }
    }

    // Changer le RoomId
    public void ChangeRoomId(int newRoomId)
    {
        _roomID = newRoomId;
        Debug.Log($"RoomId changé pour {newRoomId}");
    }

    // Accès aux données pour l'ennemi
    public int GetHealth() { return _health; }
    public float GetMoveSpeed() { return _moveSpeed; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FriendlyProjectile"))
        {
            TakeDamage(1); 
            Destroy(collision.gameObject);
        }
    }
    private void TakeDamage(int amount)
    {
        _health -= amount;

        if (_enemyAnimation != null)
            _enemyAnimation.FlashRed(); // effet visuel

        if (_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        // Tu peux ajouter des effets visuels, sons, score, etc.
        Destroy(gameObject);
    }
}