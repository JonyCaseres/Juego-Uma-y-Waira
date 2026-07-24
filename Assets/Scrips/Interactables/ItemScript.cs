using UnityEngine;

public class ItemMundo : MonoBehaviour
{
    public ItemSO item;
    public int cantidad = 1;

    private InventarioJugador jugador;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (jugador == null)
                jugador = other.GetComponent<InventarioJugador>();

            if (jugador != null)
            {
                jugador.AgregarItem(item, cantidad);

                Destroy(gameObject);
            }
        }
    }
}