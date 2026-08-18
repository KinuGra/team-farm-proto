using UnityEngine;
using System.Collections.Generic;

public class Campfire : MonoBehaviour
{
    public float interactDistance = 2f;

    // レシピ: 入力アイテム -> 出力アイテム
    [SerializeField] private ItemData riceItem;
    [SerializeField] private ItemData wakameItem;
    [SerializeField] private ItemData cookedRiceItem;
    [SerializeField] private ItemData cookedWakameItem;

    private Transform player;
    private HighlightController highlight;

    // キャッシュ: 入力アイテムデータ -> 出力アイテムデータ
    private Dictionary<ItemData, ItemData> recipes = new Dictionary<ItemData, ItemData>();

    void Start()
    {
        highlight = GetComponent<HighlightController>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }

        // レシピを初期化
        if (riceItem != null && cookedRiceItem != null)
        {
            recipes[riceItem] = cookedRiceItem;
        }
        if (wakameItem != null && cookedWakameItem != null)
        {
            recipes[wakameItem] = cookedWakameItem;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool isNear = distance <= interactDistance;
        bool hasAnyItem = HasAnyCookableItem();

        bool canCook = isNear && hasAnyItem;

        highlight.SetHighlight(canCook);

        if (canCook && Input.GetKeyDown(KeyCode.F))
        {
            TryCook();
        }
    }

    bool HasAnyCookableItem()
    {
        foreach (var recipe in recipes)
        {
            if (InventoryManager.instance.GetItemCount(recipe.Key) > 0)
                return true;
        }
        return false;
    }

    void TryCook()
    {
        foreach (var recipe in recipes)
        {
            if (InventoryManager.instance.UseItem(recipe.Key, 1))
            {
                InventoryManager.instance.AddItem(recipe.Value);
                Debug.Log(recipe.Key.ItemName + " → " + recipe.Value.ItemName);
                return;
            }
        }
    }
}
