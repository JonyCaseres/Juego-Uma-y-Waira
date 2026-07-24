using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerOnCollision : MonoBehaviour
{
    [Header("Scene to load")]
    [Tooltip("Si está marcado, se usará Scene Name; si no, se usará Build Index.")]
    public bool useSceneName = true;
    public string sceneName;
    public int sceneBuildIndex = 0;

    [Header("Filtrado")]
    [Tooltip("Etiqueta del objeto que puede activar el cambio de escena. Dejar vacío para no filtrar.")]
    public string requiredTag = "Player";

    [Header("Física")]
    [Tooltip("Marcar si el collider está configurado como Trigger (OnTrigger). Si no, usará OnCollision.")]
    public bool useTrigger = true;
    [Tooltip("Usar las versiones 2D de los callbacks (OnTriggerEnter2D/OnCollisionEnter2D).")]
    public bool is2D = true;

    [Header("Opciones")]
    [Tooltip("Cargar la escena de forma asíncrona.")]
    public bool loadAsync = false;
    [Tooltip("Evitar recargas múltiples tras la primera activación.")]
    public bool singleUse = true;

    bool hasActivated = false;

    void TryActivate(GameObject other)
    {
        if (hasActivated && singleUse) return;
        if (!string.IsNullOrEmpty(requiredTag) && other.tag != requiredTag) return;

        if (loadAsync)
        {
            if (useSceneName)
                SceneManager.LoadSceneAsync(sceneName);
            else
                SceneManager.LoadSceneAsync(sceneBuildIndex);
        }
        else
        {
            if (useSceneName)
                SceneManager.LoadScene(sceneName);
            else
                SceneManager.LoadScene(sceneBuildIndex);
        }

        hasActivated = true;
    }

    // 3D Trigger
    void OnTriggerEnter(Collider other)
    {
        if (useTrigger && !is2D) TryActivate(other.gameObject);
    }

    // 3D Collision
    void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger && !is2D) TryActivate(collision.gameObject);
    }

    // 2D Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (useTrigger && is2D) TryActivate(other.gameObject);
    }

    // 2D Collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && is2D) TryActivate(collision.gameObject);
    }
}