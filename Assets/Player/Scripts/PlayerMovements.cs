using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [Header("Speed")]
    public float speed = 4.5f; // Vitesse du joueur
    public float speedDecrease = 1.025f; // Sert a diminuer la vitesse du joueur (facteur de glissance)
    public bool canMove = true; // Détermine si le joueur peut bouger ou non

    [Header("velocity axis")]
    public float horizontalVelocity; // Vitesse horizontale
    public float verticalVelocity; // Vitesse verticale

    [Header("Renderer")]
    SpriteRenderer spriteRenderer;

    [Header("Stats")]
    public PlayerStats stats; 

    void Start()
    {
        horizontalVelocity = 0;
        verticalVelocity = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (stats == null)
        {
            stats = GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (canMove)
        {
            Movements();
        }
    }
    /// <summary>
    /// Gère les mouvements du joueur en 2D avec un effet de glissement.
    /// Le joueur peut se déplacer horizontalement avec les touches Q (gauche) et D (droite),
    /// ainsi que verticalement avec les touches Z (haut) et S (bas).
    /// Si aucune touche n'est enfoncée, la vitesse du joueur diminue progressivement jusqu'à s'arrêter.
    /// </summary>
    private void Movements()
    {
        // HORIZONTAL
        if (Input.GetKey(KeyCode.A))
        {
            if (horizontalVelocity > -1f)
            {
                horizontalVelocity -= .1f;
                spriteRenderer.flipX = true;
            }
        }

        else if (Input.GetKey(KeyCode.D))
        {
            if (horizontalVelocity < 1f)
            {
                horizontalVelocity += .1f;
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            if (horizontalVelocity < 0f)
            {
                horizontalVelocity = horizontalVelocity / speedDecrease;
                if (horizontalVelocity > -0.1)
                {
                    horizontalVelocity = 0;
                }
            }
            else if (horizontalVelocity > 0f)
            {
                horizontalVelocity = horizontalVelocity / speedDecrease;
                if (horizontalVelocity < 0.1)
                {
                    horizontalVelocity = 0;
                }
            }
        }

        // VERTICAL
        if (Input.GetKey(KeyCode.W))
        {
            if (verticalVelocity < 1f)
            {
                verticalVelocity += .1f;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if (verticalVelocity > -1f)
            {
                verticalVelocity -= .1f;
            }
        }
        else
        {
            if (verticalVelocity < 0f)
            {
                verticalVelocity = verticalVelocity / speedDecrease;
                if (verticalVelocity > -0.1)
                {
                    verticalVelocity = 0;
                }
            }
            else if (verticalVelocity > 0f)
            {
                verticalVelocity = verticalVelocity / speedDecrease;
                if (verticalVelocity < 0.1)
                {
                    verticalVelocity = 0;
                }
            }
        }

        Vector2 direction = new Vector2(horizontalVelocity, verticalVelocity);
        transform.Translate(direction * speed * stats.playerMoveSpeed * Time.deltaTime);
    }

}