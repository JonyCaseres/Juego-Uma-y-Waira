using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Guardar la casilla de origen
        originalParent = transform.parent;

        // Mover temporalmente al Root para que se dibuje por encima de todo
        transform.SetParent(transform.root);

        // Desactivar el raycast para detectar lo que hay debajo mientras se arrastra
        canvasGroup.blocksRaycasts = false;

        // Hacerlo un poco transparente
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Hacer que el ítem siga la posición del ratón/cursor
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Restaurar opacidad y raycast
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Intentar obtener la casilla sobre la que se soltó el ítem
        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();

        // Si se soltó sobre otro ítem en vez de la casilla directamente
        if (dropSlot == null)
        {
            GameObject dropItem = eventData.pointerEnter;
            if (dropItem != null)
            {
                dropSlot = dropItem.GetComponentInParent<Slot>();
            }
        }

        Slot originalSlot = originalParent.GetComponent<Slot>();
        /*
        if (dropSlot != null)
        {
            // SI LA CASILLA DESTINO YA TIENE UN ÍTEM (INTERCAMBIO)
            if (dropSlot.currentItem != null)
            {
                // Mover el ítem existente en el destino a la casilla original
                dropSlot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSlot.currentItem;

                // Centrar el ítem intercambiado
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                // Si la casilla destino estaba vacía, dejar vacía la casilla original
                originalSlot.currentItem = null;
            }

            // Mover el ítem actual a la casilla destino
            transform.SetParent(dropSlot.transform);
            dropSlot.currentItem = gameObject;
        }
        else
        {
            // SI SE SOLTÓ FUERA DE UNA CASILLA: Regresar a la casilla de origen
            transform.SetParent(originalParent);
        }

        // Centrar el ítem dentro de su casilla final
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;}*/

    }
}
