using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{

    // Variable contenant le fichier de notre ennemi (contenant les caractèristiques : nom, description, Sprite, etc.)
    public EnemyData enemyData;

    private void Start()
    {
       
// Si enemyData n'est pas vide, on va charger l'ennemi basé sur les infos qu'on a
        if (enemyData != null)
        {
            LoadEnemy(enemyData);
        }
    }

    private void LoadEnemy(EnemyData _data)
    {
        // Supprime tous les child de l'empty s'il y en a 
        foreach (Transform child in transform)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        // Fait apparaitre le Sprite de notre ennemi et le configure
        GameObject visuals = new GameObject();
        SpriteRenderer renderer = visuals.AddComponent<SpriteRenderer>();
        renderer.sprite = enemyData.enemySprite;
        visuals.transform.position = transform.position;
        visuals.transform.localScale = enemyData.enemyScale;
        

    }

    
    
}