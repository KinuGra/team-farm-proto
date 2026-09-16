using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ItemSlotPanel : MonoBehaviour
{
    private Image iconImage;
    private TextMeshProUGUI countText;
    [SerializeField] private int slotIndex = 0;

    public int SlotIndex => slotIndex;

    private void Awake()
    {
        foreach (var img in GetComponentsInChildren<Image>())
        {
            if (img.gameObject != gameObject)
            {
                iconImage = img;
                break;
            }
        }
        countText = GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log(iconImage);
    }

    public void SetItem(ItemStack stack)
    {
        if (stack != null && stack.ItemData != null)
        {
            iconImage.sprite = stack.ItemData.Icon;
            iconImage.enabled = true;

            if (countText != null)
            {
                countText.text = stack.Quantity > 1 ? stack.Quantity.ToString() : "";
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        if (iconImage != null)
        {
            iconImage.sprite = null;
            iconImage.enabled = false;
        }

        if (countText != null)
        {
            countText.text = "";
        }
    }
}