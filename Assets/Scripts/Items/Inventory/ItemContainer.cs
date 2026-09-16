using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// アイテムを受け取る側の最小契約。
/// </summary>
public interface IItemReceiver
{
    bool CanReceive(ItemData itemData, int quantity);
    bool Receive(ItemData itemData, int quantity);
}

/// <summary>
/// アイテムを収納するコンテナの基本クラス
/// </summary>
/// 
public class ItemContainer : MonoBehaviour, IItemReceiver
{
    [SerializeField] private int maxSlots = 5;  // スロット数の上限
    
    private List<ItemStack> items = new List<ItemStack>();

    /// <summary>
    /// 現在のスロット数を取得
    /// </summary>
    public int CurrentSlots => items.Count;
    public int MaxSlots => maxSlots;

    /// <summary>
    /// アイテムリストが変更されたときに呼び出されるイベント
    /// </summary>
    public event Action OnListChanged;

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
    /// 複数のアイテムをまとめて追加できるか判定する（実際には追加しない）
    /// </summary>
    public bool CanAddItems(List<ItemData> itemDatas, List<int> quantities)
    {
        if (itemDatas == null || quantities == null || itemDatas.Count != quantities.Count)
            return false;

        List<ItemStack> simulatedItems = new List<ItemStack>();
        foreach (ItemStack item in items)
        {
            simulatedItems.Add(new ItemStack(item.ItemData, item.Quantity));
        }

        for (int i = 0; i < itemDatas.Count; i++)
        {
            ItemData itemData = itemDatas[i];
            int quantity = quantities[i];

            if (itemData == null || quantity <= 0)
                return false;

            int remaining = quantity;
            foreach (ItemStack stack in simulatedItems)
            {
                if (stack.ItemData == itemData && !stack.IsFull)
                {
                    remaining = stack.AddQuantity(remaining);
                    if (remaining == 0) break;
                }
            }

            while (remaining > 0 && simulatedItems.Count < maxSlots)
            {
                int addAmount = Mathf.Min(remaining, itemData.MaxStack);
                simulatedItems.Add(new ItemStack(itemData, addAmount));
                remaining -= addAmount;
            }

            if (remaining > 0)
                return false;
        }

        return true;
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

                NotifyListChanged(); // 変更通知
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

        NotifyListChanged(); // 変更通知
        return remaining;  // 追加できなかった分を返す
    }

    public bool CanReceive(ItemData itemData, int quantity)
    {
        return CanAddItem(itemData, quantity);
    }

    public bool Receive(ItemData itemData, int quantity)
    {
        return AddItem(itemData, quantity) == 0;
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

        NotifyListChanged(); // 変更通知
        return true;
    }

    /// <summary>
    /// アイテムを使用する
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
        NotifyListChanged(); // 変更通知

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

    /// <summary>
    /// アイテムリストが変更されたことを通知する
    /// </summary>
    protected void NotifyListChanged()
    {
        OnListChanged?.Invoke();
    }
}
