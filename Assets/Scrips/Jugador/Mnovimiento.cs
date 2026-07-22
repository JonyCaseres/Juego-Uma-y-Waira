using UnityEngine;
using UnityEngine.InputSystem; // Necesario para InputValue

public class Movimiento2D : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Vector2 direccion;
    private bool mirandoDerecha = true;

    [Header("Movimiento")]
    [SerializeField] private float velocidadMovimiento = 6f;

    [Header("Salto")]
    [SerializeField] private float fuerzaSalto = 6f;

    [Header("Detección de Suelo")]
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private Transform controladorSuelo;
    [SerializeField] private Vector2 dimensionesCaja = new Vector2(0.5f, 0.2f);
    private bool enSuelo;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Verificar si está en el suelo usando una caja de colisión
        enSuelo = Physics2D.OverlapBox(controladorSuelo.position, dimensionesCaja, 0f, capaSuelo);

        // Ajustar la orientación del personaje
        AjustarRotacion(direccion.x);
    }

    private void FixedUpdate()
    {
        // Aplicar velocidad horizontal conservando la velocidad vertical
        rb2D.linearVelocity = new Vector2(direccion.x * velocidadMovimiento, rb2D.linearVelocity.y);
    }

    // Callback que Unity llama automáticamente desde PlayerInput
    public void OnMove(InputValue value)
    {
        direccion = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && enSuelo)
        {
            rb2D.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    private void AjustarRotacion(float direccionX)
    {
        if (direccionX > 0 && !mirandoDerecha)
        {
            Girando();
        }
        else if (direccionX < 0 && mirandoDerecha)
        {
            Girando();
        }
    }

    private void Girando()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    private void OnDrawGizmos()
    {
        if (controladorSuelo != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(controladorSuelo.position, dimensionesCaja);
        }
    }
}

