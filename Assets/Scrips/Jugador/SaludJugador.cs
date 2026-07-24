using UnityEngine;
using UnityEngine.UI;

public class BarraCorazones : MonoBehaviour
{
    public Image[] corazones;

    private int cantidadActual;


    void Start()
    {
        cantidadActual = corazones.Length;
        ActualizarUI();
    }


    public void RecibirDaño()
    {
        if (cantidadActual <= 0)
            return;


        cantidadActual--;

        ActualizarUI();
    }


    public void Recuperar()
    {
        if (cantidadActual >= corazones.Length)
            return;


        cantidadActual++;

        ActualizarUI();
    }


    void ActualizarUI()
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].enabled = i < cantidadActual;
        }
    }
}