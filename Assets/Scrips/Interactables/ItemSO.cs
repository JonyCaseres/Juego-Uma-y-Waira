using UnityEngine;

[CreateAssetMenu(menuName = "Inventario/Item")]
public class ItemSO : ScriptableObject
{
    public string nombre;

    [TextArea]
    public string descripcion;

    public Sprite icono;

    public int cantidadMax = 99;
}