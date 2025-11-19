using UnityEngine;


/// <summary>
/// Este controlador ejecutar� el movimiento de la c�mara del jugador
/// est�ndar de codificaci�n:
/// https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/language-specification/documentation-comments
/// </summary>
public class CameraPlayer : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0, 0, -10);
    /// <summary>
    /// Start is called once before the first execution of Update after the MonoBehaviour is created
    /// </summary>
    void Start()
    {

    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        transform.position = player.transform.position + offset;

    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.z = 0;
        transform.position = pos;
    }
}

