using UnityEngine;

public class Mnovimiento : MonoBehaviour
{
    public float velocidad = 5f;
    public float fuerzaSalto = 8f;
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector2 movimiento;
    [SerializeField]
    private bool mirandoDerecha = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float direccionX = Input.GetAxisRaw("Horizontal");
        movimiento = new Vector2(direccionX, 0f);

        if (Input.GetKeyDown(KeyCode.Space) && rb != null && Mathf.Abs(rb.linearVelocity.y) < 0.01f)
        {
            Saltar();
        }

        AjustarMovimiento(direccionX);
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(movimiento.x * velocidad, rb.linearVelocity.y);
        }
    }

    private void AjustarMovimiento(float direccionX)
    {
        if (direccionX > 0f && !mirandoDerecha)
        {
            Girar();
        }
        else if (direccionX < 0f && mirandoDerecha)
        {
            Girar();
        }
    }

    private void Saltar()
    {
        if (rb != null)
        {
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}
