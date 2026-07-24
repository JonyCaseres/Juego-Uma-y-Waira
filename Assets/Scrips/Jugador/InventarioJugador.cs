using UnityEngine;

public class InventarioJugador : MonoBehaviour
{
    public SlotInveentario[] slots = new SlotInveentario[5];

    public int slotSeleccionado = 0;

    private void Awake()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i] = new SlotInveentario();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Seleccionar(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            Seleccionar(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            Seleccionar(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            Seleccionar(3);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            Seleccionar(4);
    }

    void Seleccionar(int indice)
    {
        slotSeleccionado = indice;

        Debug.Log("Slot seleccionado: " + (indice + 1));
    }

    public void AgregarItem(ItemSO item, int cantidad)
    {
        //Si ya existe, aumenta la cantidad
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item)
            {
                slots[i].cantidad += cantidad;
                return;
            }
        }

        //Si no existe, busca un espacio vacío
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].Vacio())
            {
                slots[i].item = item;
                slots[i].cantidad = cantidad;
                return;
            }
        }

        Debug.Log("Inventario lleno");
    }

    public bool TieneItem(ItemSO item)
    {
        if (item == null) return false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == item && slots[i].cantidad > 0)
                return true;
        }

        return false;
    }

    public ItemSO ItemSeleccionado()
    {
        return slots[slotSeleccionado].item;
    }
}

