using UnityEngine;

public enum TipoMina { Pequeña, Mediana, Grande }

public class Mina : MonoBehaviour, IInteractable
{
    [Header("Configuración de la mina")]
    public ItemSO mineral;
    public TipoMina tipoMina;

    [SerializeField] private int cantidadPorInteraccion;
    [SerializeField] private int cantidadTotal;

    private bool isMinando = false;

    private void Start()
    {
        switch (tipoMina)
        {
            case TipoMina.Pequeña: cantidadTotal = 10; break;
            case TipoMina.Mediana: cantidadTotal = 20; break;
            case TipoMina.Grande: cantidadTotal = 40; break;
        }
    }

    public bool CanInteract() => cantidadTotal > 0 && !isMinando;

    public void Interact(GameObject jugador)
    {
        if (!CanInteract()) return;

        isMinando = true;

        // Procesar recolección directamente sin minijuego.
        cantidadPorInteraccion = ObtenerCantidadAleatoria();
        cantidadPorInteraccion = Mathf.Min(cantidadPorInteraccion, cantidadTotal);

        if (cantidadPorInteraccion > 0)
        {
            InventarioJugador inv = jugador.GetComponent<InventarioJugador>();
            if (inv != null)
            {
                inv.AgregarItem(mineral, cantidadPorInteraccion);
                Debug.Log($"Recolectado {cantidadPorInteraccion} de {mineral.nombre}.");
                cantidadTotal -= cantidadPorInteraccion;
            }
            else
            {
                Debug.LogWarning("Jugador sin InventarioJugador; no se pudo agregar el mineral.");
            }
        }

        if (cantidadTotal <= 0)
        {
            Debug.Log("La mina se ha agotado.");
            gameObject.SetActive(false);
        }

        isMinando = false;
    }

    private int ObtenerCantidadAleatoria()
    {
        switch (tipoMina)
        {
            case TipoMina.Pequeña: return Random.Range(1, 4); // Cambiado a 1 para asegurar que dé algo si acierta
            case TipoMina.Mediana: return Random.Range(2, 6);
            case TipoMina.Grande: return Random.Range(3, 8);
            default: return 0;
        }
    }
}
