using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField, Tooltip("Liste des sprites vers le haut")]
    private Sprite[] upSprites;
    [SerializeField, Tooltip("Liste des sprites vers le bas")]
    private Sprite[] downSprites;
    [SerializeField, Tooltip("Liste des sprites vers la gauche")]
    private Sprite[] leftSprites;
    [SerializeField, Tooltip("Liste des sprites vers la droite")]
    private Sprite[] rightSprites;

    public float _animationSpeed = 0.1f;
    private float _timer;
    private int _currentFrame;

    private SpriteRenderer spriteRenderer;

    private Sprite[] currentSprites; // Stocke la dernière direction utilisée

    private float _hVelocity;
    private float _vVelocity;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentSprites = downSprites;
    }

    void Update()
    {
        Vector2 velocity = new Vector2(_hVelocity, _vVelocity);

        if (velocity.magnitude < 0.1f)
        {
            // Idle
            if (currentSprites != null && currentSprites.Length > 0)
            {
                spriteRenderer.sprite = currentSprites[0];
            }
            return;
        }

        // Animation de marche
        _timer += Time.deltaTime;
        if (_timer >= _animationSpeed)
        {
            _timer = 0f;
            _currentFrame = (_currentFrame + 1) % 4;

            // Choisir la direction dominante
            if (Mathf.Abs(velocity.y) >= Mathf.Abs(velocity.x))
            {
                currentSprites = velocity.y > 0 ? upSprites : downSprites;
            }
            else
            {
                currentSprites = velocity.x > 0 ? rightSprites : leftSprites;
            }

            if (currentSprites.Length == 4)
            {
                spriteRenderer.sprite = currentSprites[_currentFrame];
            }
        }
    }

    /// <summary>
    /// Met à jour les vitesses horizontale et verticale utilisées pour déterminer l'animation.
    /// Cette méthode est appelée par un autre script (ex: PlayerController) pour synchroniser l'animation avec le mouvement réel.
    /// </summary>
    /// <param name="h">Vitesse horizontale du joueur.</param>
    /// <param name="v">Vitesse verticale du joueur.</param>
    public void UpdateAnimation(float h, float v)
    {
        _hVelocity = h;
        _vVelocity = v;
    }
}

