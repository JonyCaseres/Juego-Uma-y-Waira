using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlot : MonoBehaviour
{
    [SerializeField]
    private Image icono;
    [SerializeField]
    public TextMeshProUGUI cantidad;
    [SerializeField]
    public Image fondo;

    public void Mostrar(ItemSO item, int cant, bool seleccionado)
    {
        if (item == null)
        {
            icono.enabled = false;
            cantidad.text = "";
        }
        else
        {
            icono.enabled = true;
            icono.sprite = item.icono;
            cantidad.text = cant.ToString();
        }

        fondo.color = seleccionado ? Color.yellow : Color.white;
    }
}
