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

    [Header("Param�tres d'animation")]
    [SerializeField] private float _animationSpeed = 0.1f; // Vitesse de l'animation

    private float _timer;
    private int _currentFrame;

    private SpriteRenderer _spriteRenderer;

    private Sprite[] _currentSprites;

    private Coroutine _flashCoroutine;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>(); // R�cup�re le composant SpriteRenderer de l'objet
        _currentSprites = _downSprites; // Direction par d�faut
    }

    public void SetAnimation(float horizontalVelocity, float verticalVelocity)
    {
        // D�termine la direction dominante
        if (Mathf.Abs(verticalVelocity) >= Mathf.Abs(horizontalVelocity))
        {
            _currentSprites = verticalVelocity > 0 ? _upSprites : _downSprites;
        }
        else
        {
            _currentSprites = horizontalVelocity > 0 ? _rightSprites : _leftSprites;
        }

        // Animation de d�placement
        _timer += Time.deltaTime;
        if (_timer >= _animationSpeed)
        {
            _timer = 0f;
            _currentFrame = (_currentFrame + 1) % _currentSprites.Length;
            _spriteRenderer.sprite = _currentSprites[_currentFrame]; // Mise � jour de l'animation
        }
    }

    // Changement de couleur lors des d�gats

    public void FlashRed(float duration = 1f)
    {
        if (_flashCoroutine != null)
            StopCoroutine(_flashCoroutine);

        _flashCoroutine = StartCoroutine(FlashRedCoroutine(duration));
    }

    private IEnumerator FlashRedCoroutine(float duration)
    {
        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(duration);

        _spriteRenderer.color = originalColor;
    }
}
