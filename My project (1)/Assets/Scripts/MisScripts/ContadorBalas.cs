using UnityEngine;
using TMPro;

public class ContadorBalas : MonoBehaviour
{
    public TextMeshProUGUI Text; // Usa TextMeshProUGUI en vez de TextMeshPro

    // Método estático para actualizar el texto
    public static ContadorBalas instance;

    void Awake()
    {
        instance = this; // Guardamos referencia global
    }

    public static void UpdateBalas()
    {
        if (instance != null && instance.Text != null)
        {
            instance.Text.text = $"Balas activas: {Bullet.balas:00}";
        }
    }
}