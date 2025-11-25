using UnityEngine;

public class EnemyShooterPreview : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private EnemyShooter enemyShooter;
    [SerializeField] private Bullet.BulletType currentType = Bullet.BulletType.Straight;

    [Header("Preview Settings")]
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField, Range(0f, 3f)] private float simulationTime = 2f;
    [SerializeField] private int simulationSteps = 50;
    [SerializeField] private float pointSize = 0.1f;
    [SerializeField] private bool showTrajectory = true;

    private void OnDrawGizmos()
    {
        // Auto-asigna si está en el mismo GameObject
        if (enemyShooter == null)
        {
            enemyShooter = GetComponent<EnemyShooter>();
        }

        if (enemyShooter == null) return; // No dibuja si no hay shooter

        Transform firePoint = enemyShooter.firePoint;
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;

        // INTERPRETA LO QUE HACE EnemyShooter.Shoot()
        switch (currentType)
        {
            case Bullet.BulletType.Straight:
                DrawFireStraight(origin);
                break;

            case Bullet.BulletType.CurvedSine:
                DrawFireSpread(origin);
                break;

            case Bullet.BulletType.Homing:
                DrawFireHoming(origin);
                break;

            case Bullet.BulletType.Flower:
                DrawFireFlower(origin);
                break;
        }
    }

    // Simula FireStraight()
    private void DrawFireStraight(Vector3 origin)
    {
        // LEE directamente del shooter
        int bulletAmount = enemyShooter.bulletAmount;
        float startAngle = enemyShooter.startAngle;
        float endAngle = enemyShooter.endAngle;

        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            Gizmos.color = Color.cyan;
            DrawBulletTrajectory(origin, dir, Bullet.BulletType.Straight);

            angle += angleStep;
        }
    }

    // Simula FireSpread() - DISPARA 2 BALAS POR ÁNGULO
    private void DrawFireSpread(Vector3 origin)
    {
        // LEE directamente del shooter
        int bulletAmount = enemyShooter.bulletAmount;
        float startAngle = enemyShooter.startAngle;
        float endAngle = enemyShooter.endAngle;

        float angleStep = (endAngle - startAngle) / bulletAmount;
        float angle = startAngle;

        for (int i = 0; i < bulletAmount; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Primera bala: CurvedSine (Amarillo)
            Gizmos.color = Color.yellow;
            DrawBulletTrajectory(origin, dir, Bullet.BulletType.CurvedSine);

            // Segunda bala: CurvedCosine (Magenta)
            Gizmos.color = Color.magenta;
            DrawBulletTrajectory(origin, dir, Bullet.BulletType.CurvedCosine);

            angle += angleStep;
        }
    }

    // Simula FireHoming()
    private void DrawFireHoming(Vector3 origin)
    {
        Gizmos.color = Color.red;
        DrawBulletTrajectory(origin, Vector2.up, Bullet.BulletType.Homing);
    }

    // Simula FireFlower() - CÍRCULO COMPLETO CON PÉTALOS
    private void DrawFireFlower(Vector3 origin)
    {
        // LEE directamente del shooter
        int petalCount = enemyShooter.petalCount;
        float angleStep = 360f / petalCount;

        for (int i = 0; i < petalCount; i++)
        {
            float angle = angleStep * i;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            // Cada pétalo con su fase
            float phaseOffset = (i * 2f * Mathf.PI / petalCount);

            Gizmos.color = Color.green;
            DrawBulletTrajectoryWithPhase(origin, dir, Bullet.BulletType.Flower, phaseOffset);
        }
    }

    private void DrawBulletTrajectory(Vector3 origin, Vector2 direction, Bullet.BulletType type)
    {
        DrawBulletTrajectoryWithPhase(origin, direction, type, 0f);
    }

    private void DrawBulletTrajectoryWithPhase(Vector3 origin, Vector2 direction, Bullet.BulletType type, float phaseOffset)
    {
        Vector3 previousPos = origin;
        Transform target = type == Bullet.BulletType.Homing ?
            FindObjectOfType<PlayerController>()?.transform : null;

        float timeStep = simulationTime / simulationSteps;
        Vector3 currentPos = origin;
        float simulatedTime = 0f;

        for (int step = 0; step <= simulationSteps; step++)
        {
            // USA EL COMPORTAMIENTO REAL de Bullet.cs CON phase offset
            Vector2 movement = Bullet.CalculateMovement(type, direction, target, currentPos, simulatedTime + phaseOffset);

            Vector3 displacement = (Vector3)(movement * bulletSpeed * timeStep);
            currentPos += displacement;

            // Dibuja la trayectoria
            if (showTrajectory)
            {
                Color trajColor = Gizmos.color;
                trajColor.a = 0.3f;
                Gizmos.color = trajColor;
                Gizmos.DrawLine(previousPos, currentPos);
            }

            // Dibuja puntos a intervalos
            if (step % 10 == 0)
            {
                Color pointColor = Gizmos.color;
                pointColor.a = 1f;
                Gizmos.color = pointColor;
                Gizmos.DrawSphere(currentPos, pointSize);
            }

            previousPos = currentPos;
            simulatedTime += timeStep;
        }

        // Punto final
        Color finalColor = Gizmos.color;
        finalColor.a = 1f;
        Gizmos.color = finalColor;
        Gizmos.DrawSphere(currentPos, pointSize * 2f);
    }
}