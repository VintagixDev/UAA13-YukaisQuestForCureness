using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStats : MonoBehaviour
{
    [Header("Statistiques du joueur")]
    public int playerHP = 6;
    public double playerDamage = 5;
    public float playerMoveSpeed = 1f;

    [Header("Statistiques des projectiles")]
    public double playerProjectileSize = 1;
    public double playerProjectileReach = 1;
    public double playerProjectileSpeed = 1;

    [Header("Statistiques Objets")]
    public int playerGolds = 0;
    public int playerKeys = 0;

  

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
