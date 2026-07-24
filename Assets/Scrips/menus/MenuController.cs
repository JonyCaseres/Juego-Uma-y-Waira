using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;

    void Start()
    {
        if (menuCanvas != null)
        {
            menuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Si el menú no está activo y el juego ya está pausado por otra razón, no permitir abrir el menú
            if (!menuCanvas.activeSelf && PauseController.isGamePaused)
            {
                return;
            }

            if (menuCanvas != null)
            {
                menuCanvas.SetActive(!menuCanvas.activeSelf);
                
                // Si el menú está abierto se pausa el juego, si está cerrado se despausa
                PauseController.SetPaused(menuCanvas.activeSelf);
            }
        }
    }
}
