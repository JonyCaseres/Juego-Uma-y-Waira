using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount = 16;
    public GameObject[] itemPrefabs;

    void Start()
    {
        InitializeInventory();
    }

    void InitializeInventory()
    {
        for (int i = 0; i < slotCount; i++)
        {
            // Instanciar la casilla como hija del panel del inventario
            GameObject slotObj = Instantiate(slotPrefab, inventoryPanel.transform);
            Slot slot = slotObj.GetComponent<Slot>();

            // Si hay un ítem disponible para esta casilla en las pruebas
            if (i < itemPrefabs.Length)
            {
                GameObject item = Instantiate(itemPrefabs[i], slot.transform);
                
                // Centrar el ítem dentro de la casilla
                RectTransform itemRect = item.GetComponent<RectTransform>();
                itemRect.anchoredPosition = Vector2.zero;

                // Asignar la referencia en la casilla
                slot.currentItem = item;
            }
        }
    }
}