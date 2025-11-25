using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] private GameObject pooledEnemyBullet; // Prefab de bala enemiga
    [SerializeField] private GameObject pooledPlayerBullet; // Prefab de bala jugador

    private List<GameObject> bullets; // Lista única para TODAS las balas
    private bool notEnoughBulletsInPool = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        bullets = new List<GameObject>();
    }

    // Para balas enemigas
    public GameObject LoadEnemyBullet()
    {
        // Busca una bala enemiga inactiva (por tag)
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy && bullets[i].CompareTag("Bullet"))
            {
                return bullets[i];
            }
        }

        // Si no hay, crea una nueva
        if (notEnoughBulletsInPool)
        {
            GameObject newBullet = Instantiate(pooledEnemyBullet);
            newBullet.SetActive(false);
            newBullet.tag = "Bullet";
            bullets.Add(newBullet);
            return newBullet;
        }

        return null;
    }

    // Para balas del jugador
    public GameObject LoadPlayerBullet()
    {
        // Busca una bala del jugador inactiva (por tag)
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy && bullets[i].CompareTag("BulletP"))
            {
                return bullets[i];
            }
        }

        // Si no hay, crea una nueva
        if (notEnoughBulletsInPool)
        {
            GameObject newBullet = Instantiate(pooledPlayerBullet);
            newBullet.SetActive(false);
            newBullet.tag = "BulletP";
            bullets.Add(newBullet);
            return newBullet;
        }

        return null;
    }

    // MANTÉN ESTE MÉTODO para compatibilidad (usa enemigo por defecto)
    public GameObject LoadBullets()
    {
        return LoadEnemyBullet();
    }
}