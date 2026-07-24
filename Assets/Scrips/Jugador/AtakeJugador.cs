using UnityEngine;

public class AtakeJugador : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform controladorAtaque;

    [Header("Ataque")]
    [SerializeField] private float radioAtaque = 1f;
    [SerializeField] private int danoAtaque = 1;
    [SerializeField] private float tiempoEntreAtaques = 0.5f;
    [SerializeField] private KeyCode teclaAtaque;
    [SerializeField] private string etiquetaAtacable = "Atacable";
    [SerializeField] private LayerMask capaAtacable = ~0;

    private float tiempoUltimoAtaque;
    private const string ANIMACION_ATAQUE = "Picar Illa";

    private void Start()
    {
        tiempoUltimoAtaque = -tiempoEntreAtaques;

        if (controladorAtaque == null)
            controladorAtaque = transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(teclaAtaque))
        {
            Debug.Log("Botón de ataque presionado");
            IntentarAtacar();
        }
    }

    private void IntentarAtacar()
    {
        if (Time.time < tiempoUltimoAtaque + tiempoEntreAtaques)
            return;

        tiempoUltimoAtaque = Time.time;
        Atacar();
    }

    private void Atacar()
    {
        Debug.Log("Ejecutando ataque...");

        if (animator != null)
        {
            animator.SetTrigger(ANIMACION_ATAQUE);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaAtacable);
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag(etiquetaAtacable))
                continue;

            if (hit.TryGetComponent(out Atacable atacable))
            {
                atacable.RecibirGolpe(danoAtaque);
                Debug.Log($"Golpeado: {hit.name} con {danoAtaque} de daño");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (controladorAtaque == null)
            controladorAtaque = transform;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(controladorAtaque.position, radioAtaque);

        Collider2D[] hits = Physics2D.OverlapCircleAll(controladorAtaque.position, radioAtaque, capaAtacable);
        foreach (Collider2D hit in hits)
        {
            if (!hit.CompareTag(etiquetaAtacable))
                continue;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(controladorAtaque.position, hit.transform.position);
            Gizmos.DrawWireSphere(hit.transform.position, 0.15f);
        }
    }
}

