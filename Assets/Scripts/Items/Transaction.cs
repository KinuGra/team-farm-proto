using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// トランザクションアイテム（入力）
/// </summary>
[System.Serializable]
public class TransactionInput
{
    public ItemData itemData;
    public int quantity = 1;
}

[System.Serializable]
public class TransactionOutput
{
    public ItemData itemData;
    public int quantity = 1;
}

public enum ItemType
{
    Food,
    Material,
    Tool,
    Trade,
    Others,
}
/// <summary>
/// トランザクション定義（複数入力 -> 1つの出力）
/// 調理、取引など汎用的に使用可能
/// ScriptableObject として使用可能
/// </summary>
[CreateAssetMenu(menuName = "Items/Transaction")]
public class Transaction : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private ItemType category;
    [SerializeField] private string transactionName;
    [SerializeField] private List<TransactionInput> inputs;
    [SerializeField] private List<TransactionOutput> outputs;

    public int Id => id;
    public ItemType Category => category;
    public string TransactionName => transactionName;
    public List<TransactionInput> Inputs => inputs;
    public List<TransactionOutput> Outputs => outputs;

    /// <summary>
    /// トランザクションの消費内容と獲得内容を表示用に取得
    /// </summary>
    public string GetResultDescription()
    {
        List<string> inputDescriptions = new List<string>();
        foreach (var input in inputs)
        {
            if (input != null && input.itemData != null)
                inputDescriptions.Add($"{input.itemData.ItemName} x{input.quantity}");
        }

        List<string> outputDescriptions = new List<string>();
        foreach (var output in outputs)
        {
            if (output != null && output.itemData != null)
                outputDescriptions.Add($"{output.itemData.ItemName} x{output.quantity}");
        }

        return $"消費: {string.Join(", ", inputDescriptions)} → 獲得: {string.Join(", ", outputDescriptions)}";
    }

    /// <summary>
    /// このレシピが実行可能か確認
    /// </summary>
    public bool CanExecute(InventoryManager inventory)
    {
        if (inventory == null || inputs == null || inputs.Count == 0 || outputs == null || outputs.Count == 0)
            return false;

        // すべての入力アイテムが揃っているか確認
        foreach (var input in inputs)
        {
            if (input == null || input.itemData == null)
                return false;

            int count = inventory.GetItemCount(input.itemData);
            if (count < input.quantity)
                return false;
        }

        List<ItemData> outputItems = new List<ItemData>();
        List<int> outputQuantities = new List<int>();
        for (int i = 0; i < outputs.Count; i++)
        {
            if (outputs[i] == null || outputs[i].itemData == null || outputs[i].quantity <= 0)
                return false;

            outputItems.Add(outputs[i].itemData);
            outputQuantities.Add(outputs[i].quantity);
        }

        // 全出力をまとめて追加できるか確認
        return inventory.CanAddItems(outputItems, outputQuantities);
    }

    /// <summary>
    /// このレシピを実行（入力を消費して出力を追加）
    /// </summary>
    public bool Execute(InventoryManager inventory)
    {
        if (!CanExecute(inventory))
            return false;

        // 入力アイテムを消費
        foreach (var input in inputs)
        {
            inventory.RemoveItem(input.itemData, input.quantity);
        }

        // 出力アイテムを追加
        foreach (var output in outputs)
        {
            inventory.AddItem(output.itemData, output.quantity);
        }

        return true;
    }

    private void OnValidate()
    {
        if (inputs == null)
            return;

        foreach (var input in inputs)
        {
            if (input != null && input.quantity < 1)
                input.quantity = 1;
        }

        if (outputs == null)
            return;

        foreach (var output in outputs)
        {
            if (output != null && output.quantity < 1)
                output.quantity = 1;
        }
    }
}