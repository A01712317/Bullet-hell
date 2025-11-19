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
    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    /// 
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
        transform.Translate(Vector3.up * Time.deltaTime * speed * forwardInput);

        transform.Translate(Vector3.right * horizontalInput * Time.deltaTime * turnSpeed);
        
    }
}
