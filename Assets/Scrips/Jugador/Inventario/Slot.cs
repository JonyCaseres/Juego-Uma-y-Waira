using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    [Header("UI del Slot")]
    public Image icono;
    public TMP_Text cantidadTexto;

    private ItemSO itemActual;

    public void Mostrar(ItemSO item, int cantidad)
    {
        if (item == null)
        {
            Limpiar();
            return;
        }

        itemActual = item;

        // Mostrar icono
        icono.sprite = item.icono;
        icono.enabled = true;

        // Mostrar cantidad solo si es mayor a 1
        if (cantidad > 1)
        {
            cantidadTexto.gameObject.SetActive(true);
            cantidadTexto.text = "x" + cantidad;
        }
        else
        {
            cantidadTexto.gameObject.SetActive(false);
        }
    }


    public void Limpiar()
    {
        itemActual = null;

        icono.sprite = null;
        icono.enabled = false;

        cantidadTexto.gameObject.SetActive(false);
    }


    public ItemSO ObtenerItem()
    {
        return itemActual;
    }
}