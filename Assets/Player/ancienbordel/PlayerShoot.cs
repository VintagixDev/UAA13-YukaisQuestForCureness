using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    /// <summary>
    /// (Up/Down/Left/Right)Spawn : Indique la direction du player
    /// </summary>
    
    [Header("Direction")]
    public Transform UpSpawn;
    public Transform DownSpawn;
    public Transform LeftSpawn;
    public Transform RightSpawn;

    /// <summary>
    /// Bullet : Fait r�f�rence aux projectiles
    /// </summary>

    [Header("Game object")]
    public GameObject Bullet;

    /// <summary>
    /// bulletSpeed : Vitesse du projectile
    /// bulletRate : Sert a empecher de tirer trop de projectiles � la seconde
    /// bulletRange : D�termine la port�e du projectile
    /// </summary>

    [Header("Projectil stats")]
    public float bulletSpeed = 1f;
    public float bulletRate = 2f;
    public float bulletRange = 1.5f;
    private bool canShoot = true;
    void Update()
    {
        if (canShoot)
        {
            // Gauche
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                Shoot(LeftSpawn, new Vector2(-1, 0));
            }
            // Droite
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Shoot(RightSpawn, new Vector2(1, 0));
            }
            // Haut
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Shoot(UpSpawn, new Vector2(0, 1));
            }
            // Bas
            if (Input.GetKey(KeyCode.DownArrow))
            {
                Shoot(DownSpawn, new Vector2(0, -1));
            }
        }
        
    }
    // Fonction qui tire le projectile
    private void Shoot(Transform spawnPoint, Vector2 shootDirection)
    {
        // appel de la fonction limitante
        canShoot = false;
        StartCoroutine(BulletRate());
        // lancement du projectile
        GameObject bullet = Instantiate(Bullet, spawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = shootDirection*bulletSpeed;
        bullet.GetComponent<BulletShoot>()._timeToDeath = bulletRange;
    }

    // Fonction qui limite le nombre de projectiles lanc�s
    IEnumerator BulletRate()
    {
        yield return new WaitForSeconds(1f/bulletRate);
        canShoot = true;
    }
}
