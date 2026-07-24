using System.Collections.Generic;
using UnityEngine;

public class SimpleDamageUI : MonoBehaviour
{
    [Tooltip("Arrastra los GameObjects con SpriteRenderer (cada cuadrito).")]
    [SerializeField] private List<SpriteRenderer> slots = new List<SpriteRenderer>(5);
    private int current;

    private void Awake()
    {
        current = 0;
        if (slots != null)
        {
            foreach (var s in slots) if (s != null && s.gameObject.activeSelf) current++;
        }
        SyncSlots();
    }

    public void OnDamage(int amount = 1)
    {
        if (slots == null || amount <= 0) return;
        for (int i = 0; i < amount; i++)
        {
            if (current <= 0) break;
            current--;
            DeactivateLastActive();
        }
    }

    public void OnHeal(int amount = 1)
    {
        if (slots == null || amount <= 0) return;
        for (int i = 0; i < amount; i++)
        {
            if (current >= slots.Count) break;
            ActivateNextInactive();
            current++;
        }
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

    private void DeactivateLastActive()
    {
        if (slots == null) return;
        for (int i = slots.Count - 1; i >= 0; i--)
        {
            var s = slots[i];
            if (s == null) continue;
            if (s.gameObject.activeSelf)
            {
                s.gameObject.SetActive(false);
                return;
            }
        }
    }

    private void ActivateNextInactive()
    {
        if (slots == null) return;
        for (int i = 0; i < slots.Count; i++)
        {
            var s = slots[i];
            if (s == null) continue;
            if (!s.gameObject.activeSelf)
            {
                s.gameObject.SetActive(true);
                return;
            }
        }
    }
}