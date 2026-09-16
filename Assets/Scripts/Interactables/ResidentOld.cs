using UnityEngine;
using System.Collections.Generic;

public class ResidentOld: MonoBehaviour
{
    public float interactDistance = 2.0f;

    [SerializeField] private List<ItemData> tradableItems = new List<ItemData>();

    private Transform player;
    private HighlightController highlight;
    private int itemCount;

    void Start()
    {
        highlight = GetComponent<HighlightController>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool isNear = distance <= interactDistance;
        bool hasAnyItem = HasAnyTradableItem();

        bool canTrade = isNear && hasAnyItem;

        highlight.SetHighlight(canTrade);

        if (canTrade && Input.GetKeyDown(KeyCode.F))
        {
            TryTrade();
        }
    }

    bool HasAnyTradableItem()
    {
        foreach (var item in tradableItems)
        {
            if (item != null && InventoryManager.instance.GetItemCount(item) > 0)
                return true;
        }
        return false;
    }

    void TryTrade()
    {
        foreach (var item in tradableItems)
        {
            if (item == null) continue;
            itemCount = InventoryManager.instance.GetItemCount(item);
            if (InventoryManager.instance.UseItem(item, itemCount))
            {
                Debug.Log(item.ItemName + "を与えました！");
                return;
            }
        }
    }
}
