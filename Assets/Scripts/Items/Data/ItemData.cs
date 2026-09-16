using UnityEngine;

/// <summary>
/// アイテムの定義・属性を管理するScriptableObject
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [SerializeField] private int itemId;
    [SerializeField] private string itemName;
    [SerializeField] private string description;
    [SerializeField] private int maxStack = 99;
    [SerializeField] private int value = 0;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject model;
    public int ItemId => itemId;
    public string ItemName => itemName;
    public string Description => description;
    public int MaxStack => maxStack;
    public int Value => value;
    public Sprite Icon => icon;
    public GameObject Model => model;
    private void OnValidate()
    {
        if (maxStack < 1) maxStack = 1;
        if (value < 0) value = 0;
    }
}
