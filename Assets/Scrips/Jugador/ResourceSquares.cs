using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ResourceSquares : MonoBehaviour
    {
        [SerializeField] private List<Image> slots = new List<Image>(5);
        [SerializeField] private Sprite slotFull;
        [SerializeField] private Sprite slotEmpty;
        [SerializeField] private Text valueText;

        public void SetValue(int current, int max)
        {
            if (slots == null || slots.Count == 0) return;
            int slotCount = slots.Count;
            int filled = Mathf.RoundToInt(((float)current / Mathf.Max(1, max)) * slotCount);
            filled = Mathf.Clamp(filled, 0, slotCount);

            for (int i = 0; i < slotCount; i++)
            {
                var img = slots[i];
                if (img == null) continue;
                img.sprite = (i < filled) ? slotFull : slotEmpty;
                img.enabled = true;
            }

            if (valueText != null)
                valueText.text = $"{current}/{max}";
        }

        public void ClearAll()
        {
            if (slots == null) return;
            foreach (var img in slots) if (img != null) img.enabled = false;
            if (valueText != null) valueText.text = "";
        }
    }
}