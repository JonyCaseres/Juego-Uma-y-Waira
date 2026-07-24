using UnityEngine;

public class ItemMundo : MonoBehaviour
{
    public ItemSO item;
    public int cantidad = 1;

    private InventarioJugador jugador;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Colisionando con: " + other.name);

        if (!other.CompareTag("Player"))
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Se presionó F");

            if (jugador == null)
                jugador = other.GetComponent<InventarioJugador>();

            if (jugador != null)
            {
                Debug.Log("Inventario encontrado");

                jugador.AgregarItem(item, cantidad);

                Destroy(gameObject);
            }
            else
            {
                Debug.LogError("No se encontró InventarioJugador en el Player");
            }
        }
    }
}