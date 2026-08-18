using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// プレイヤーのインベントリを管理する（シングルトン）
/// </summary>
public class InventoryManager : MonoBehaviour
{
    // シングルトン
    public static InventoryManager instance;

    [SerializeField] private int maxSlots = 5;  // スロット数の上限
    
    private List<ItemStack> items = new List<ItemStack>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 現在のスロット数を取得
    /// </summary>
    public int CurrentSlots => items.Count;
    public int MaxSlots => maxSlots;

    /// <summary>
    /// インベントリ内のアイテムリストを取得
    /// </summary>
    public IReadOnlyList<ItemStack> Items => items.AsReadOnly();

    /// <summary>
    /// アイテムが全て追加できるか判定する（実際には追加しない）
    /// </summary>
    public bool CanAddItem(ItemData itemData, int quantity = 1)
    {
        if (itemData == null || quantity <= 0) return false;

        int remaining = quantity;

        // 既存スタックで追加できるか確認
        foreach (ItemStack stack in items)
        {
            if (stack.ItemData == itemData && !stack.IsFull)
            {
                int canAdd = stack.ItemData.MaxStack - stack.Quantity;
                remaining -= canAdd;

                if (remaining <= 0) return true;
            }
        }

        // 新しいスタックが必要な数を計算
        int slotsNeeded = Mathf.CeilToInt((float)remaining / itemData.MaxStack);
        int availableSlots = maxSlots - items.Count;

        return slotsNeeded <= availableSlots;
    }

    /// <summary>
    /// アイテムを追加する。スロット満杯の場合は追加できない分を返す
    /// </summary>
    public int AddItem(ItemData itemData, int quantity = 1)
    {
        if (itemData == null || quantity <= 0) return quantity;

        int remaining = quantity;

        // 既存スタックに追加
        foreach (ItemStack stack in items)
        {
            if (stack.ItemData == itemData && !stack.IsFull)
            {
                int overflow = stack.AddQuantity(remaining);
                remaining = overflow;

                if (remaining == 0) return 0;
            }
        }

        // 新しいスタックを作成
        while (remaining > 0 && items.Count < maxSlots)
        {
            int addAmount = Mathf.Min(remaining, itemData.MaxStack);
            
            ItemStack newStack = new ItemStack(itemData, addAmount);
            items.Add(newStack);
            remaining -= addAmount;
        }

        return remaining;  // 追加できなかった分を返す
    }

    /// <summary>
    /// アイテムが削除できるか判定する（実際には削除しない）
    /// </summary>
    public bool CanRemoveItem(ItemData itemData, int quantity = 1)
    {
        if (itemData == null || quantity <= 0) return false;

        int count = GetItemCount(itemData);
        return count >= quantity;
    }

    /// <summary>
    /// アイテムを削除する。失敗時はfalseを返す
    /// 複数スタックにまたがる削除に対応
    /// </summary>
    public bool RemoveItem(ItemData itemData, int quantity = 1)
    {
        if (itemData == null || quantity <= 0) return false;

        // 先に削除できるか判定
        if (!CanRemoveItem(itemData, quantity)) return false;

        // 削除できる場合は実際に削除
        int remaining = quantity;

        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].ItemData == itemData)
            {
                if (items[i].Quantity >= remaining)
                {
                    // このスタックで全て削除完了
                    items[i].RemoveQuantity(remaining);
                    remaining = 0;
                }
                else
                {
                    // このスタック全体を削除
                    remaining -= items[i].Quantity;
                    items[i].RemoveQuantity(items[i].Quantity);
                }

                // 空のスタックを削除
                if (items[i].IsEmpty)
                {
                    items.RemoveAt(i);
                }

                if (remaining == 0) break;
            }
        }

        return true;
    }

    /// <summary>
    /// アイテムを使用する（RemoveItem の別名）
    /// </summary>
    public bool UseItem(ItemData itemData, int quantity = 1)
    {
        return RemoveItem(itemData, quantity);
    }

    /// <summary>
    /// アイテムの数を取得
    /// </summary>
    public int GetItemCount(ItemData itemData)
    {
        if (itemData == null) return 0;

        int count = 0;
        foreach (ItemStack stack in items)
        {
            if (stack.ItemData == itemData)
            {
                count += stack.Quantity;
            }
        }
        return count;
    }

    /// <summary>
    /// インベントリをクリア
    /// </summary>
    public void Clear()
    {
        items.Clear();
    }

    /// <summary>
    /// スロット数の上限を変更
    /// </summary>
    public void SetMaxSlots(int newMaxSlots)
    {
        maxSlots = Mathf.Max(1, newMaxSlots);
        
        // スロット数を超えている場合は削除
        while (items.Count > maxSlots)
        {
            ItemStack lastStack = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
        }
    }
}
