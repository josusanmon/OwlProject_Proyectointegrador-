using UnityEngine;

public class CamaraSigueCombi : MonoBehaviour
{
    [Header("Objetivo")]
    public Transform objetivo;                       // arrastra aqui Combi1

    [Header("Posicion de la camara (relativa a la combi)")]
    // X = lados, Y = altura, Z = adelante/atras.
    // Si el frente de la combi es +Z, atras es -Z.
    // Si el frente es +X, usa (-10, 4, 0).
    public Vector3 desplazamiento = new Vector3(0f, 4f, -10f);

    [Header("Ajustes")]
    public float alturaDeMira = 1.5f;                // punto de la combi al que mira
    public float suavizado = 5f;                     // mas alto = camara mas pegada

    void LateUpdate()
    {
        if (objetivo == null) return;

        // Solo se usa el giro en Y de la combi, para que la camara no se incline
        Quaternion giro = Quaternion.Euler(0f, objetivo.eulerAngles.y, 0f);
        Vector3 posicionDeseada = objetivo.position + giro * desplazamiento;

        float t = 1f - Mathf.Exp(-suavizado * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, posicionDeseada, t);

        Vector3 puntoMira = objetivo.position + Vector3.up * alturaDeMira;
        Quaternion rotacionDeseada = Quaternion.LookRotation(puntoMira - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, t);
    }
}
