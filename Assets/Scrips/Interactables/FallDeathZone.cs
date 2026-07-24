using UnityEngine;

public class FallDeathZone : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private bool useTrigger = true;
    [Tooltip("Tiempo mínimo entre activaciones por el mismo objeto")]
    [SerializeField] private float cooldown = 0.5f;

    private readonly System.Collections.Generic.Dictionary<GameObject, float> lastHit = new System.Collections.Generic.Dictionary<GameObject, float>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!useTrigger) return;
        HandleCollision(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (useTrigger) return;
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider2D col)
    {
        if (col == null) return;
        GameObject target = col.attachedRigidbody != null ? col.attachedRigidbody.gameObject : col.gameObject;
        if (target == null) return;
        if (!target.CompareTag(playerTag)) return;

        if (IsOnCooldown(target)) return;
        RegisterHit(target);

        var barra = target.GetComponentInChildren<BarraCorazones>() ?? FindObjectOfType<BarraCorazones>();
        if (barra == null)
        {
            Debug.LogWarning($"FallDeathZone: no se encontró 'BarraCorazones' para el jugador '{target.name}'.");
            return;
        }

        int current = barra.CurrentLives;
        if (current <= 0) return;

        barra.RecibirDaño(current);
        Debug.Log($"FallDeathZone: Jugador '{target.name}' cayó. Se aplicaron {current} daños. Vidas restantes: {barra.CurrentLives}/{barra.MaxLives}");
    }

    private bool IsOnCooldown(GameObject obj)
    {
        if (lastHit.TryGetValue(obj, out var t))
            return Time.time - t < cooldown;
        return false;
    }

    private void RegisterHit(GameObject obj)
    {
        lastHit[obj] = Time.time;
    }
}