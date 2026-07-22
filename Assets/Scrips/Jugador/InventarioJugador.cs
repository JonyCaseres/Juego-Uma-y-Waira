using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SlotInventario
{
    public ItemSO item;
    public int cantidad;

    public SlotInventario(ItemSO item, int cantidad)
    {
        this.item = item;
        this.cantidad = cantidad;
    }
}

public class InventarioJugador : MonoBehaviour
{
    [Header("Economía")]
    public int monedas = 10;

    // Métodos auxiliares para gestionar monedas
    public bool PuedePagar(int cantidad)
    {
        return monedas >= cantidad;
    }

    public bool Pagar(int cantidad)
    {
        if (monedas >= cantidad)
        {
            monedas -= cantidad;
            return true;
        }
        return false;
    }

    public void AñadirMonedas(int cantidad)
    {
        monedas += cantidad;
    }

    private const int maxSlots = 5;
    public List<SlotInventario> slots = new List<SlotInventario>(maxSlots);

    public void AgregarItem(ItemSO nuevoItem, int cantidad = 1)
    {
        if (nuevoItem == null) return;

        SlotInventario slotExistente = slots.Find(s => s.item == nuevoItem);

        if (slotExistente != null)
        {
            slotExistente.cantidad = Mathf.Min(slotExistente.cantidad + cantidad, nuevoItem.cantidadMax);
            Debug.Log($"Actualizado: {nuevoItem.nombre} x{cantidad}");
            return;
        }

        if (slots.Count >= maxSlots)
        {
            Debug.LogWarning($"No hay espacio en el inventario. Máximo {maxSlots} slots permitidos.");
            return;
        }

        slots.Add(new SlotInventario(nuevoItem, cantidad));
        Debug.Log($"Agregado: {nuevoItem.nombre} x{cantidad}");
    }

    public void RemoverItem(ItemSO item, int cantidad = 1)
    {
        SlotInventario slot = slots.Find(s => s.item == item);

        if (slot != null)
        {
            slot.cantidad -= cantidad;
            if (slot.cantidad <= 0)
                slots.Remove(slot);

            Debug.Log($"Removido: {item.nombre} x{cantidad}");
        }
    }

    public bool TieneItem(ItemSO item)
    {
        if (item == null) return false;
        foreach (var s in slots)
        {
            if (s.item == item && s.cantidad > 0) return true;
        }
        return false;
    }
}

