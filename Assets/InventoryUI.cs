using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject panel;
    public Transform content;
    public GameObject slotPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panel.SetActive(!panel.activeSelf);
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        // 全部削除
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        // 追加
        foreach (string item in Inventory.instance.items)
        {
            GameObject slot = Instantiate(slotPrefab, content);

            TMP_Text text = slot.GetComponentInChildren<TMP_Text>();
            text.text = item;
        }
    }
}
