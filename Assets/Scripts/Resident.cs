using UnityEngine;
using System.Collections.Generic;

public class Resident: MonoBehaviour
{
    public float interactDistance = 2.0f;

    private Transform player;
    private HighlightController highlight;
    private int itemCount;
    private List<string> tradableItems = new List<string>()
    {
        "CookedRice" ,"CookedWakame" 
    };

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
            if (Inventory.instance.GetItemCount(item) > 0)
                return true;
        }
        return false;
    }

    // 未実装: 報酬, 食わせるアイテムの選別, 完了
    void TryTrade()
    {
        foreach (var item in tradableItems)
        {
            itemCount = Inventory.instance.GetItemCount(item);
            if (Inventory.instance.UseItem(item, itemCount))
            {
                
                Debug.Log(item+"を与えました！");
                return;
            }
        }
    }
}
