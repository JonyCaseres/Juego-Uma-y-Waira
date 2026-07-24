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

    // Nombre del Trigger en el Animator
    private const string ANIMACION_ATAQUE = "Picar Illa";

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
            Debug.Log("Botón de ataque presionado");
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
        Debug.Log("Ejecutando Picar Illa...");

        // Reproducir la animación
        if (animator != null)
        {
            animator.SetTrigger(ANIMACION_ATAQUE);
        }

        // Detectar objetos dentro del área de ataque
        Collider2D[] objetosTocados = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque);

        foreach (Collider2D objeto in objetosTocados)
        {
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