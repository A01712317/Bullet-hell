using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Este controllador actualizar� los eventos del veh�culo del jugador
/// Estandar de codificaci�n: 
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class PlayerController : MonoBehaviour
{
    //Variables c�mara
    public Camera mainCamera;

    public float speed = 5.0f;
    public float turnSpeed = 0.0f;
    public float horizontalInput;
    public float forwardInput;

    public Vector2 minBounds = new Vector2(-8, -5);
    public Vector2 maxBounds = new Vector2(8, 5);

    public int maxLives = 5;
    private int currentLives;
    public float damagedSpeed = .5f; // velocidad reducida al recibir golpe
    public float damageDuration = .5f; // tiempo de ralentización
    private bool isDamaged = false;

    public Transform firePoint; // punto desde donde dispara el jugador


    public AudioClip ShotClip;     // sonido de destrucción
    private AudioSource audioSource;

    private void Start()
    {
        currentLives = maxLives;
        audioSource = GetComponent<AudioSource>(); // inicializa el AudioSource
    }

    void LateUpdate()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(transform.position.y, minBounds.y, maxBounds.y);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {

        forwardInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed * forwardInput * damagedSpeed);

            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed * damagedSpeed);
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * speed * forwardInput);

            transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * speed);
        }


        if(Input.GetKeyDown(KeyCode.Space))
        {
            FirePlayerBullet();
            PlaySound(ShotClip);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Detecta colisión con balas
        if (collision.CompareTag("Bullet"))
        {
            TakeDamage();
            collision.gameObject.SetActive(false); // desactiva la bala
        }
    }

    void TakeDamage()
    {
        currentLives--;

        // Aplica efecto de ralentización
        if (!isDamaged)
            StartCoroutine(DamageEffect());

        // Si ya no quedan vidas, destruye la nave
        if (currentLives <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DamageEffect()
    {
        isDamaged = true;
        float originalSpeed = speed;
        speed = damagedSpeed;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = Color.red;

        yield return new WaitForSeconds(damageDuration);

        sr.color = Color.white;
        speed = originalSpeed;
        isDamaged = false;
    }

    void FirePlayerBullet()
    {
        GameObject bullet = ObjectPool.Instance.LoadPlayerBullet();

        if (bullet != null)
        {
            bullet.transform.position = firePoint != null ? firePoint.position : transform.position;
            bullet.SetActive(true);

            Bullet b = bullet.GetComponent<Bullet>();
            b.bulletType = Bullet.BulletType.Player;   // tipo de bala
            b.SetShootDirection(Vector2.up);           // dirección hacia arriba
            bullet.tag = "BulletP";
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

}
