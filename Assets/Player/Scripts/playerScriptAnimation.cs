using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScriptAnimation : MonoBehaviour
{
    public Sprite[] upSprites;
    public Sprite[] downSprites;
    public Sprite[] leftSprites;
    public Sprite[] rightSprites;

    public float animationSpeed = 0.1f;
    private float timer;
    private int currentFrame;

    private SpriteRenderer spriteRenderer;
    private PlayerMovements playerMovements;

    private Sprite[] currentSprites; // Stocke la dernière direction utilisée

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovements = GetComponent<PlayerMovements>();
        timer = 0f;
        currentFrame = 0;
        currentSprites = downSprites; // Direction par défaut
    }

    void Update()
    {
        Vector2 velocity = new Vector2(playerMovements.horizontalVelocity, playerMovements.verticalVelocity);

        if (velocity.magnitude < 0.1f)
        {
            // Le joueur est à l’arrêt → idle sur la première frame de la dernière direction
            if (currentSprites != null && currentSprites.Length > 0)
            {
                spriteRenderer.sprite = currentSprites[0];
            }
            return;
        }

        // Le joueur est en mouvement → animation de marche
        timer += Time.deltaTime;
        if (timer >= animationSpeed)
        {
            timer = 0f;
            currentFrame = (currentFrame + 1) % 4;

            // Détermine la direction dominante
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
                spriteRenderer.sprite = currentSprites[currentFrame];
            }
        }
    }
}

