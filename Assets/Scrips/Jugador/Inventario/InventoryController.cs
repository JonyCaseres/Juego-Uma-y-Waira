using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public InventarioJugador inventarioJugador;

    [Header("Inventario")]
    public RectTransform inventoryPanel;      // Panel que se mueve
    public Transform contenedorSlots;         // Panel Inventario (Grid Layout)

    [Header("Slots")]
    public GameObject slotPrefab;
    public int slotCount = 16;

    [Header("Animación")]
    public Vector2 posicionVisible = Vector2.zero;
    public Vector2 posicionOculta = new Vector2(700, 0);
    public float velocidad = 10f;

    private bool abierto = false;
    private Vector2 destino;

    void Start()
    {
        // El panel empieza oculto
        inventoryPanel.anchoredPosition = posicionOculta;
        destino = posicionOculta;

        InitializeInventory();
        inventarioJugador.AlActualizarInventario += ActualizarUI;

        ActualizarUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }

        inventoryPanel.anchoredPosition = Vector2.Lerp(
            inventoryPanel.anchoredPosition,
            destino,
            Time.deltaTime * velocidad
        );
    }

    public void ToggleInventory()
    {
        abierto = !abierto;
        destino = abierto ? posicionVisible : posicionOculta;
    }

    private Slot[] slotsUI;

    void InitializeInventory()
    {
        foreach (Transform hijo in contenedorSlots)
        {
            Destroy(hijo.gameObject);
        }

        slotsUI = new Slot[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, contenedorSlots);

            slotsUI[i] = slotObj.GetComponent<Slot>();
        }
    }
    void ActualizarUI()
    {
        // Limpiar todos los slots
        for (int i = 0; i < slotsUI.Length; i++)
        {
            slotsUI[i].Limpiar();
        }

        // Mostrar los objetos que tiene el jugador
        for (int i = 0; i < inventarioJugador.slots.Count; i++)
        {
            SlotInventario slot = inventarioJugador.slots[i];

            slotsUI[i].Mostrar(slot.item, slot.cantidad);
        }
    }
}