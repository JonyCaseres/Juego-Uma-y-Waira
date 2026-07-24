using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerOnCollision : MonoBehaviour
{
    [Header("Destino")]
    [Tooltip("Usar nombre de escena si está activado; en caso contrario usar build index.")]
    public bool useSceneName = true;
    public string sceneName;
    public int sceneBuildIndex = 0;

    [Header("Filtrado")]
    [Tooltip("Etiqueta requerida del otro objeto (vacío = no filtrar).")]
    public string requiredTag = "Player";

    [Header("Física")]
    [Tooltip("Si el collider está como Trigger, activar OnTriggerEnter*; si no, OnCollisionEnter*.")]
    public bool useTrigger = true;
    [Tooltip("Usar callbacks 2D (OnTriggerEnter2D / OnCollisionEnter2D) en lugar de 3D.")]
    public bool is2D = true;

    [Header("Opciones")]
    [Tooltip("Cargar la escena de forma asíncrona.")]
    public bool loadAsync = false;
    [Tooltip("Si true evita múltiples activaciones.")]
    public bool singleUse = true;

    private bool hasActivated = false;

    void TryChangeScene(GameObject other)
    {
        if (hasActivated && singleUse) return;
        if (!string.IsNullOrEmpty(requiredTag) && other != null && other.tag != requiredTag) return;

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
        if (useTrigger && !is2D) TryChangeScene(other.gameObject);
    }

    // 3D Collision
    void OnCollisionEnter(Collision collision)
    {
        if (!useTrigger && !is2D) TryChangeScene(collision.gameObject);
    }

    // 2D Trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        if (useTrigger && is2D) TryChangeScene(other.gameObject);
    }

    // 2D Collision
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!useTrigger && is2D) TryChangeScene(collision.gameObject);
    }
}