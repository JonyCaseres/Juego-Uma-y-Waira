using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SpikeDamage : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float hitCooldown = 1f;
    [SerializeField] private string playerTag = "Player";

    private SimpleDamageUI simpleUI;
    private readonly Dictionary<GameObject, float> lastHitTime = new Dictionary<GameObject, float>(8);

    private void Awake()
    {
        simpleUI = FindObjectOfType<SimpleDamageUI>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = ResolveTargetGameObject(other);
        if (target == null) return;
        if (!target.CompareTag(playerTag)) return;

        if (IsOnCooldown(target)) return;
        RegisterHit(target);
        ApplyDamageToPlayer(target);
    }

    private GameObject ResolveTargetGameObject(Collider2D col)
    {
        if (col == null) return null;
        if (col.attachedRigidbody != null) return col.attachedRigidbody.gameObject;
        return col.gameObject;
    }

    private bool IsOnCooldown(GameObject target)
    {
        if (lastHitTime.TryGetValue(target, out var t))
            return Time.time - t < hitCooldown;
        return false;
    }

    private void RegisterHit(GameObject target)
    {
        lastHitTime[target] = Time.time;
    }

    private void ApplyDamageToPlayer(GameObject player)
    {
        // 1) Actualizar UI simple si existe
        if (simpleUI != null)
        {
            simpleUI.OnDamage(damage);
        }

        // 2) Intentar notificar al componente de salud del jugador por reflexión
        // Buscar componentes mono que puedan manejar daño
        var monos = player.GetComponents<MonoBehaviour>();
        foreach (var mb in monos)
        {
            if (mb == null) continue;
            var type = mb.GetType();

            // métodos comunes a intentar (con y sin parámetro int)
            var methodNames = new[] { "RecibirDaño", "RecibirDanio", "TakeDamage", "ReceiveDamage", "ApplyDamage", "Damage", "Hurt" };
            foreach (var name in methodNames)
            {
                var m = type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (m == null) continue;
                var ps = m.GetParameters();
                try
                {
                    if (ps.Length == 0)
                        m.Invoke(mb, null);
                    else if (ps.Length == 1 && (ps[0].ParameterType == typeof(int) || ps[0].ParameterType == typeof(float)))
                        m.Invoke(mb, new object[] { Convert.ChangeType(damage, ps[0].ParameterType) });
                    else if (ps.Length == 1 && ps[0].ParameterType == typeof(object))
                        m.Invoke(mb, new object[] { damage });
                }
                catch { }
                // asumo que el componente manejará el daño; no rompo para intentar múltiples
            }
        }

        // 3) Si no hay métodos, intentar modificar campos/properties comunes (currentHealth, vida, etc.)
        foreach (var mb in monos)
        {
            if (mb == null) continue;
            var t = mb.GetType();
            try
            {
                var prop = t.GetProperty("currentHealth") ?? t.GetProperty("CurrentHealth") ?? t.GetProperty("vidaActual") ?? t.GetProperty("VidaActual");
                if (prop != null && prop.CanRead && prop.CanWrite)
                {
                    int cur = Convert.ToInt32(prop.GetValue(mb));
                    prop.SetValue(mb, Math.Max(0, cur - damage));
                    break;
                }

                var field = t.GetField("currentHealth") ?? t.GetField("CurrentHealth") ?? t.GetField("vidaActual") ?? t.GetField("VidaActual");
                if (field != null)
                {
                    int cur = Convert.ToInt32(field.GetValue(mb));
                    field.SetValue(mb, Math.Max(0, cur - damage));
                    break;
                }
            }
            catch { }
        }
    }

    // Exponer públicamente para daño por script (ejemplo: proyectil)
    public void HitPlayer(GameObject player)
    {
        if (player == null) return;
        if (!player.CompareTag(playerTag)) return;
        if (IsOnCooldown(player)) return;
        RegisterHit(player);
        ApplyDamageToPlayer(player);
    }
}