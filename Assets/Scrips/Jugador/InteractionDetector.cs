using UnityEngine;

public class InteractionDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private string interactionTag = "Interaction";
    [SerializeField] private GameObject interactionIcon;

    private GameObject closestInteractable;

    private void Start()
    {
        if (interactionIcon != null)
            interactionIcon.SetActive(false);
    }

    private void Update()
    {
        FindClosestInteractable();

        if (Input.GetKeyDown(KeyCode.E) && closestInteractable != null)
        {
            closestInteractable.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            if (interactionIcon != null)
                interactionIcon.SetActive(false);
        }
    }

    private void FindClosestInteractable()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        float bestDistance = float.MaxValue;
        GameObject best = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag(interactionTag))
            {
                float distance = Vector2.Distance(transform.position, hit.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = hit.gameObject;
                }
            }
        }

        closestInteractable = best;
        if (interactionIcon != null)
            interactionIcon.SetActive(closestInteractable != null);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
