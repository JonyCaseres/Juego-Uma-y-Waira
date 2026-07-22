using UnityEngine;
using UnityEngine.InputSystem;

public class AtakeJugador : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform controladorAtaque;

    [Header("Ataque")]
    [SerializeField] private float radioAtaque = 1f;
    [SerializeField] private int danoAtaque = 1;
    [SerializeField] private float tiempoEntreAtaques = 0.5f;
    private float tiempoUltimoAtaque;

    private const string ANIMACION_ATAQUE = "atacar";

    private void Start()
    {
        // Permitir atacar inmediatamente desde el inicio
        tiempoUltimoAtaque = -tiempoEntreAtaques;
    }

    // Este método será llamado automáticamente por PlayerInput
    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("Botón de ataque presionado"); // Log al presionar el botón
            IntentarAtacar();
        }
    }

    private void IntentarAtacar()
    {
        // Control de tiempo entre ataques
        if (Time.time < tiempoUltimoAtaque + tiempoEntreAtaques)
            return;

        tiempoUltimoAtaque = Time.time;
        Atacar();
    }

    private void Atacar()
    {
        Debug.Log("Ejecutando ataque..."); // Log cuando realmente se ejecuta el ataque

        // Disparar la animación de ataque
        if (animator != null)
        {
            animator.SetTrigger(ANIMACION_ATAQUE);
        }

        // Detectar colisiones dentro del área circular
        Collider2D[] objetosTocados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);

        foreach (Collider2D objeto in objetosTocados)
        {
            // Aquí podrías aplicar lógica adicional si quieres afectar otros sistemas.
            // Por ejemplo, dañar enemigos:
            /*
            if (objeto.TryGetComponent(out VidaEnemigo vidaEnemigo))
            {
                vidaEnemigo.TomarDano(danoAtaque);
                Debug.Log($"Golpeado: {objeto.name} con {danoAtaque} de daño");
            }
            */
        }
    }

    private void OnDrawGizmos()
    {
        if (controladorAtaque != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);
        }
    }
}

