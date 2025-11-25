using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 10f;
    private Vector2 shootDirection;
    public enum BulletType { Straight, CurvedSine, CurvedCosine, Homing, Flower, Player }
    public BulletType bulletType;
    public Transform target;
    public float phaseOffset = 0f;

    public static int balas = 0;

    void Start()
    {
        // Optionally initialize other properties if needed
    }

    private void OnEnable()
    {
        balas++;
        ContadorBalas.UpdateBalas();
        Invoke("Deactivate", 15f);
    }

    void Update()
    {
        Vector2 movement = CalculateMovement(bulletType, shootDirection, target, transform.position, Time.time + phaseOffset);
        transform.Translate(movement * bulletSpeed * Time.deltaTime);
    }

    // MÉTODO ESTÁTICO que puede ser usado por el Preview
    public static Vector2 CalculateMovement(BulletType type, Vector2 direction, Transform target, Vector3 currentPos, float time)
    {
        switch (type)
        {
            case BulletType.Straight:
                float curve0 = Mathf.Sin(time * 3f) + Mathf.Cos(time * 2f);
                return new Vector2(direction.x, direction.y + curve0);

            case BulletType.CurvedSine:
                float curve = Mathf.Sin(time * 5f) * 5f;
                return new Vector2(direction.x, direction.y + curve);

            case BulletType.CurvedCosine:
                float curve2 = Mathf.Cos(time * 5f) * 5f;
                return new Vector2(direction.x, direction.y + curve2);

            case BulletType.Homing:
                if (target != null)
                {
                    return (target.position - currentPos).normalized;
                }
                return direction;

            case BulletType.Flower:
                // Movimiento de flor: expande y contrae como pétalos
                float expand = Mathf.Sin(time * 8f) * 5f; // Oscila entre -2 y 2
                Vector2 perpendicular = new Vector2(-direction.y, direction.x); // Vector perpendicular
                return direction + (perpendicular * expand);

            case BulletType.Player:
                return Vector2.up;


            default:
                return direction;
        }
    }

    public void SetShootDirection(Vector2 dir)
    {
        shootDirection = dir.normalized;
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        balas--;
        ContadorBalas.UpdateBalas();
        CancelInvoke();
    }
}