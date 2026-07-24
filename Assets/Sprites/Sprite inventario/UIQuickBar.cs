using UnityEngine;

public class UIQuickBar : MonoBehaviour
{
    [SerializeField] private InventarioJugador inventario;
    [SerializeField] private UISlot[] slots;

    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Mostrar(
                inventario.slots[i].item,
                inventario.slots[i].cantidad,
                inventario.slotSeleccionado == i
            );
        }
    }
}
