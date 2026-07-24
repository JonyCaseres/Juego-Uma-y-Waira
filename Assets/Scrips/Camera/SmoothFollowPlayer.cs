using UnityEngine;

public class SmoothFollowPlayer : MonoBehaviour
{
    [Tooltip("Transform del jugador a seguir.")]
    public Transform target;

    [Tooltip("Offset de la cámara respecto al jugador.")]
    public Vector3 offset = new Vector3(0f, 1.5f, -10f);

    [Tooltip("Tiempo de suavizado para el movimiento.")]
    public float smoothTime = 0.15f;

    [Tooltip("Seguir en X / Y (útil para cámaras clamp/estáticas).")]
    public bool followX = true;
    public bool followY = true;

    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 currentPosition = transform.position;

        if (!followX) desiredPosition.x = currentPosition.x;
        if (!followY) desiredPosition.y = currentPosition.y;

        transform.position = Vector3.SmoothDamp(currentPosition, desiredPosition, ref velocity, smoothTime);
    }
}