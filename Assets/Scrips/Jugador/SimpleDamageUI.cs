using System.Collections.Generic;
using UnityEngine;

public class SimpleDamageUI : MonoBehaviour
{
    [Tooltip("Arrastra los GameObjects que tienen SpriteRenderer (cada cuadrito).")]
    [SerializeField] private List<SpriteRenderer> slots = new List<SpriteRenderer>(5);

    private int current;

    private void Awake()
    {
        var barra = FindObjectOfType<BarraCorazones>();
        if (barra != null)
        {
            barra.OnLivesChanged += OnLivesChanged;
            current = Mathf.Clamp(barra.CurrentLives, 0, slots != null ? slots.Count : 0);
        }
        else
        {
            current = slots != null ? slots.Count : 0;
        }
        SyncSlots();
    }

    private void OnDestroy()
    {
        var barra = FindObjectOfType<BarraCorazones>();
        if (barra != null)
            barra.OnLivesChanged -= OnLivesChanged;
    }

    public void OnDamage(int amount = 1)
    {
        if (slots == null || amount <= 0) return;
        current = Mathf.Max(0, current - amount);
        SyncSlots();
    }

    public void OnHeal(int amount = 1)
    {
        if (slots == null || amount <= 0) return;
        current = Mathf.Min(slots.Count, current + amount);
        SyncSlots();
    }

    public void ResetAll()
    {
        if (slots == null) return;
        current = slots.Count;
        SyncSlots();
    }

    private void SyncSlots()
    {
        if (slots == null) return;
        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];
            if (s == null) continue;
            s.gameObject.SetActive(i < current);
        }
    }

    private void OnLivesChanged(int lives, int max)
    {
        current = Mathf.Clamp(lives, 0, slots != null ? slots.Count : lives);
        SyncSlots();
    }
}