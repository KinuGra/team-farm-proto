using UnityEngine;

/// <summary>
/// インベントリ内のアイテムと数量を管理するクラス
/// </summary>
[System.Serializable]
public class ItemStack
{
    [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity;

    public ItemData ItemData => itemData;
    public int Quantity => quantity;

    public ItemStack(ItemData itemData, int quantity = 1)
    {
        this.itemData = itemData;
        this.quantity = Mathf.Max(1, quantity);
    }

    /// <summary>
    /// アイテムの数を増やす。スタック上限を超えた分を返す
    /// </summary>
    public int AddQuantity(int amount)
    {
        if (itemData == null || amount <= 0) return amount;

        int newQuantity = quantity + amount;
        int overflow = Mathf.Max(0, newQuantity - itemData.MaxStack);
        quantity = Mathf.Min(newQuantity, itemData.MaxStack);
        return overflow;
    }

    /// <summary>
    /// アイテムの数を減らす。失敗時はfalseを返す
    /// </summary>
    public bool RemoveQuantity(int amount)
    {
        if (amount <= 0 || quantity < amount) return false;
        quantity -= amount;
        return true;
    }

    /// <summary>
    /// スタックが満杯か判定
    /// </summary>
    public bool IsFull => itemData != null && quantity >= itemData.MaxStack;

    /// <summary>
    /// スタックが空か判定
    /// </summary>
    public bool IsEmpty => quantity <= 0;
}
