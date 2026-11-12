using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CombiController : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 5f;            // metros por segundo hacia adelante
    public float velocidadReversa = 3f;     // metros por segundo en reversa
    public float velocidadGiro = 90f;       // grados por segundo

    [Header("Direccion del frente (eje local)")]
    // Largo de la combi: prueba (0,0,1) o (0,0,-1)
    public Vector3 ejeFrente = new Vector3(0f, 0f, 1f);

    Rigidbody rb;
    float avance;   // 1 = W, -1 = S
    float giro;     // -1 = A, 1 = D

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // La combi solo puede girar sobre el eje Y (no se voltea)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return;

        avance = (teclado.wKey.isPressed ? 1f : 0f) - (teclado.sKey.isPressed ? 1f : 0f);
        giro   = (teclado.dKey.isPressed ? 1f : 0f) - (teclado.aKey.isPressed ? 1f : 0f);
    }

    void FixedUpdate()
    {
        // Giro (sobre el eje Y)
        Quaternion rotacion = Quaternion.Euler(0f, giro * velocidadGiro * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rotacion * rb.rotation);
        rb.angularVelocity = Vector3.zero;

        // Avance / reversa
        Vector3 direccion = transform.TransformDirection(ejeFrente.normalized);
        float v = avance > 0f ? velocidad : velocidadReversa;
        Vector3 velocidadDeseada = direccion * (avance * v);
        velocidadDeseada.y = rb.linearVelocity.y;   // conserva la gravedad
        rb.linearVelocity = velocidadDeseada;
    }
}
