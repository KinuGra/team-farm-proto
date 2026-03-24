using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public KeyCode toggleKey = KeyCode.Tab;  // Tab で開閉

    private bool isOpen = false;

    void Start()
    {
        displayText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Tab キーで表示/非表示を切り替え
        if (Input.GetKeyDown(toggleKey))
        {
            isOpen = !isOpen;
            displayText.gameObject.SetActive(isOpen);
        }

        // 開いているときだけ更新
        if (isOpen)
        {
            UpdateDisplay();
        }
    }

    void UpdateDisplay()
    {
        if (Inventory.instance == null) return;

        string text = "=== Inventory ===\n\n";

        var items = Inventory.instance.GetAllItems();

        if (items.Count == 0)
        {
            text += "Empty";
        }
        else
        {
            foreach (var item in items)
            {
                text += item.Key + "  x" + item.Value + "\n";
            }
        }

        displayText.text = text;
    }
}