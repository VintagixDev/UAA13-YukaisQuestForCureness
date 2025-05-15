using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScriptAnimation : MonoBehaviour
{
    [Header("Sprites d'animation")]
    [SerializeField] private Sprite[] _upSprites;
    [SerializeField] private Sprite[] _downSprites;
    [SerializeField] private Sprite[] _leftSprites;
    [SerializeField] private Sprite[] _rightSprites;

    [Header("Paramètres d'animation")]
    [SerializeField] private float _animationSpeed = 0.1f; // Vitesse de l'animation

    private float _timer;
    private int _currentFrame;

    private SpriteRenderer _spriteRenderer;

    private Sprite[] _currentSprites;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>(); // Récupère le composant SpriteRenderer
        _currentSprites = _downSprites; // Direction par défaut
    }

    public void SetAnimation(float horizontalVelocity, float verticalVelocity)
    {
        // Détermine la direction dominante
        if (Mathf.Abs(verticalVelocity) >= Mathf.Abs(horizontalVelocity))
        {
            _currentSprites = verticalVelocity > 0 ? _upSprites : _downSprites;
        }
        else
        {
            _currentSprites = horizontalVelocity > 0 ? _rightSprites : _leftSprites;
        }

        // Animation de déplacement
        _timer += Time.deltaTime;
        if (_timer >= _animationSpeed)
        {
            _timer = 0f;
            _currentFrame = (_currentFrame + 1) % _currentSprites.Length;
            _spriteRenderer.sprite = _currentSprites[_currentFrame]; // Mise à jour de l'animation
        }
    }
}
