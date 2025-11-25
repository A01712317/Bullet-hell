using UnityEngine;

public class EnemyShooter : MonoBehaviour
{
    public int bulletAmount = 10;
    public float startAngle = 90f;
    public float endAngle = 270f;

    public Bullet.BulletType currentType = Bullet.BulletType.Straight;
    public float fireRate = 0.5f; // tiempo entre disparos
    private float fireTimer;

    public int petalCount = 15; // Número de pétalos
    public float patternChangeRate = 5f; // cada 5 segundos cambia patrón
    private float patternTimer;

    public Transform firePoint;

    public int maxLives = 5;
    private int currentLives;

    void Update()
    {
        fireTimer += Time.deltaTime;
        patternTimer += Time.deltaTime;

        // Disparo automático
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Shoot();
        }

        // Cambio de patrón automático
        if (patternTimer >= patternChangeRate)
        {
            patternTimer = 0f;
            ChangePattern();
        }
    }

    void Shoot()
    {
        switch (currentType)
        {
            case Bullet.BulletType.Straight:
                FireStraight();
                break;

            case Bullet.BulletType.CurvedSine:
                FireSpread();
                break;

            case Bullet.BulletType.Homing:
                FireHoming();
                break;

            case Bullet.BulletType.Flower:
                FireFlower();
                break;
        }
    }

    void FireStraight()
    {
        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = ObjectPool.Instance.LoadBullets();

            if (bullet != null)
            {
                bullet.transform.position = firePoint != null ? firePoint.position : transform.position;
                bullet.SetActive(true);

                Bullet b = bullet.GetComponent<Bullet>();
                b.SetShootDirection(dir);
                b.bulletType = Bullet.BulletType.Straight;
            }
            angle += angleStep;
        }
    }

    void FireSpread()
    {
        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject bullet = ObjectPool.Instance.LoadBullets();
            GameObject bullet2 = ObjectPool.Instance.LoadBullets();

            if (bullet != null)
            {
                bullet.transform.position = firePoint != null ? firePoint.position : transform.position;
                bullet.SetActive(true);
                Bullet b = bullet.GetComponent<Bullet>();
                b.SetShootDirection(dir);
                b.bulletType = Bullet.BulletType.CurvedSine;
            }
            if (bullet2 != null)
            {
                bullet2.transform.position = firePoint.position;
                bullet2.SetActive(true);
                Bullet b2 = bullet2.GetComponent<Bullet>();
                b2.SetShootDirection(dir);
                b2.bulletType = Bullet.BulletType.CurvedCosine; // usa coseno
            }
            angle += angleStep;
        }
    }

    void FireHoming()
    {
        GameObject bullet = ObjectPool.Instance.LoadBullets();
        if (bullet != null)
        {
            bullet.transform.position = firePoint != null ? firePoint.position : transform.position;
            bullet.SetActive(true);

            Bullet b = bullet.GetComponent<Bullet>();
            b.bulletType = Bullet.BulletType.Homing;
            b.target = FindObjectOfType<PlayerController>()?.transform;
        }
    }

    void FireFlower()
    {
        // Dispara en círculo completo (360 grados) para crear pétalos
        
        float angleStep = 360f / petalCount;

        for (int i = 0; i < petalCount; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = ObjectPool.Instance.LoadBullets();
            if (bullet != null)
            {
                bullet.transform.position = firePoint != null ? firePoint.position : transform.position;
                bullet.SetActive(true);

                Bullet b = bullet.GetComponent<Bullet>();
                b.SetShootDirection(dir);
                b.bulletType = Bullet.BulletType.Flower;

                b.phaseOffset = (i * 2f * Mathf.PI / petalCount); // Distribuye las fases uniformemente
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta colisión con balas
        if (collision.CompareTag("BulletP"))
        {
            TakeDamage();
            collision.gameObject.SetActive(false); // desactiva la bala
        }
    }

    void TakeDamage()
    {
        currentLives--;

        // Si ya no quedan vidas, destruye la nave
        if (currentLives <= 0)
        {
            Destroy(gameObject);
        }
    }

    void ChangePattern()
    {
        // Alterna en ciclo recto → spread → homing → recto
        if (currentType == Bullet.BulletType.Flower)
            currentType = Bullet.BulletType.Flower;
        else if (currentType == Bullet.BulletType.Flower)
            currentType = Bullet.BulletType.Flower;
        else if (currentType == Bullet.BulletType.Flower)
            currentType = Bullet.BulletType.Flower;
        else
            currentType = Bullet.BulletType.Flower;
    }
}
