using UnityEngine;

[System.Serializable]
public class SlotInveentario
{
    public ItemSO item;
    public int cantidad;

    public bool Vacio()
    {
        return item == null;
    }

    public void Limpiar()
    {
        item = null;
        cantidad = 0;
    }
}
//