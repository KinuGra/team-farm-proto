using UnityEngine;

/// <summary>
/// 魚の定義・属性を管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "New Fish", menuName = "Fishing/FishData")]
public class FishData : ScriptableObject
{
    [SerializeField] private int fishId; // 魚のID
    [SerializeField] private string fishName; // 魚の名前
    [SerializeField] private string description; // 魚の説明
    [SerializeField] private int weight; // 魚の重さ
    [SerializeField] private ItemData item; // 魚から得られるアイテム
    public int FishId => fishId;
    public string FishName => fishName;
    public string Description => description;
    public int Weight => weight;
    public ItemData Item => item;

    private void OnValidate()
    {
        if (weight <= 0) weight = 1;
    }
}
