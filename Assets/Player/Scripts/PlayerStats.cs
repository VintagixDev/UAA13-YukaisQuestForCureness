using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStats : MonoBehaviour
{
    [Header("Statistiques du joueur")]
    public int playerHP = 6; // points de vie du joueur
    public double playerDamage = 5; // // dégât du joueur
    public float playerMoveSpeed = 1f; // Vitesse du joueur

    [Header("Statistiques des projectiles")]
    public double playerProjectileSize = 1; // Taille des projectiles du joueur
    public double playerProjectileReach = 1; // Portée des projectiles du joueur
    public double playerProjectileSpeed = 1; // Vitesse des projectiles du joueur

    [Header("Statistiques Objets")]
    public int playerGolds = 0; // Nombre de pieces // Nombre de 
    public int playerKeys = 0; // Nombre de clef du joueur

<<<<<<< HEAD
    [Header("Pièce actuelle du joueur")]
    public string currentRoom; // ID de la pièce actuelle du joueur

    public List<GameObject> playerUpgrades;
=======
    public List<Upgrade> items;
>>>>>>> 46f1d3e5a4da6d2a8f48b75879347a8391251321
}
