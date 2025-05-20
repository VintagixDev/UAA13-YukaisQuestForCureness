using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStats : MonoBehaviour 
{
    [Header("Statistiques du joueur")]
    public int playerHP ; // points de vie du joueur
    public int playerDamage; // // dégât du joueur
    public float playerMoveSpeed = 1f; // Vitesse du joueur

    [Header("Statistiques des projectiles")] // => pas besoin ici
    //public double playerProjectileSize = 1; // Taille des projectiles du joueur
    //public double playerProjectileReach = 1; // Portée des projectiles du joueur
    //public double playerProjectileSpeed = 1; // Vitesse des projectiles du joueur

    [Header("Statistiques Objets")]
    public int playerGolds = 0; // Nombre de pieces // Nombre de 
    public int playerKeys = 0; // Nombre de clef du joueur

    [Header("upgrades du joueur")]
    public List<GameObject> playerUpgrades;
}
