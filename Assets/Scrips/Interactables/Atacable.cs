using UnityEngine;

[System.Serializable]
public class ItemDrop
{
    public GameObject prefab;
    [Min(1)] public int cantidad = 1;
}

public class Atacable : MonoBehaviour
{
    [Header("Hits restantes")]
    [SerializeField] private int hits = 3; // Número de golpes que puede recibir

    [Header("Drops al destruirse")]
    [SerializeField] private ItemDrop[] itemDrops;

    // Método que recibe un golpe
    public void RecibirGolpe(int cantidad)
    {
        hits -= cantidad;
        Debug.Log($"{gameObject.name} recibió un golpe. Hits restantes: {hits}");

        if (hits <= 0)
        {
            DropItems();
            Debug.Log($"{gameObject.name} destruido");
            Destroy(gameObject);
        }
    }

    private void DropItems()
    {
        if (itemDrops == null || itemDrops.Length == 0)
            return;

        foreach (ItemDrop drop in itemDrops)
        {
            if (drop.prefab == null)
                continue;
            for (int i = 0; i < drop.cantidad; i++)
            {
                Vector3 spawnPosition = transform.position + new Vector3(
                    Random.Range(-0.3f, 0.3f),
                    Random.Range(-0.3f, 0.3f),
                    0f);

                GameObject itemObject = Instantiate(drop.prefab, spawnPosition, Quaternion.identity);
                if (itemObject == null)
                    continue;

                if (itemObject.GetComponent<ItemMundo>() == null)
                    Debug.LogWarning($"El prefab de drop '{drop.prefab.name}' no tiene ItemMundo.");
            }
        }
    }
}
