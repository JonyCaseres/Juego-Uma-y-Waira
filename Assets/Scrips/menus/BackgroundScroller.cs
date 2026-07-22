using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private float distance = 10f;
    [SerializeField] private bool useRectTransform = true;

    private Vector3 startPos;
    private RectTransform rect;
    private Vector2 startAnchoredPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (rect != null)
            startAnchoredPos = rect.anchoredPosition;
        startPos = transform.localPosition;
    }

    private void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);
        if (rect != null && useRectTransform)
            rect.anchoredPosition = startAnchoredPos + new Vector2(x, 0f);
        else
            transform.localPosition = startPos + new Vector3(x, 0f, 0f);
    }
}