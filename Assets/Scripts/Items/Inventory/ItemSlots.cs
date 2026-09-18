using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// アイテムスロットを管理するクラス
/// </summary>
/// 
public class ItemSlots : MonoBehaviour
{
    private int activeSlotIndex = 0; 
    private ItemSlotPanel[] slotPanels;

    private void Awake()
    {
        slotPanels = GetComponentsInChildren<ItemSlotPanel>();
    }

    private void OnEnable()
    {
        if (InventoryManager.instance == null) return;
        
        RegisterEvent();

        UpdateSlotDisplay(InventoryManager.instance.Items);

    }

    private void OnDisable()
    {
        if (InventoryManager.instance == null) return;
        
        InventoryManager.instance.OnListChanged -= OnInventoryChanged;
    }
    void Start()
    {
        if (InventoryManager.instance == null) return;

        RegisterEvent();
        UpdateSlotDisplay(InventoryManager.instance.Items);
    }

    void Update()
    {

        // スロット切り替えの入力処理（右） 
        if (Input.GetKeyDown(KeyCode.C))
        {   
            SetActiveIndex(activeSlotIndex + 1);
        }

        // スロット切り替えの入力処理（左）
        if (Input.GetKeyDown(KeyCode.Z))
        {   
            SetActiveIndex(activeSlotIndex - 1);
        } 

        // デバッグ
        if (Input.GetKeyDown(KeyCode.L))
        {   
            GetActiveItemData();
        } 
    }

    private void RegisterEvent()
    {
        if (InventoryManager.instance == null) return;
        InventoryManager.instance.OnListChanged -= OnInventoryChanged;
        InventoryManager.instance.OnListChanged += OnInventoryChanged;
    }
    /// <summary>
    /// インベントリの変更通知を受けた時に動く処理
    /// </summary>
    private void OnInventoryChanged()
    {
        if (InventoryManager.instance == null) return;

        int count = InventoryManager.instance.Items.Count;
        if (count <= activeSlotIndex)
        {
            activeSlotIndex = Mathf.Max(0, count - 1);
        }
        
        UpdateSlotDisplay(InventoryManager.instance.Items);

    }

    private void SetActiveIndex(int newIndex)
    {
        if (InventoryManager.instance == null) return;

        int count = InventoryManager.instance.Items.Count;
        if (count == 0)
        {
            activeSlotIndex = 0;
        } 
        else
        {
            activeSlotIndex = (newIndex%count + count) % count;
        }
        Debug.Log($"Active Slot Index: {activeSlotIndex}");
        UpdateSlotDisplay(InventoryManager.instance.Items);

    }

    /// <summary>
    /// スロットUIの描画切り出し処理
    /// </summary>
    private void UpdateSlotDisplay(IReadOnlyList<ItemStack> currentItems)
    {
        int count = InventoryManager.instance.Items.Count;
        if (count == 0) {
            ClearSlotDisplay();
            return;
        }

        int i = 0;
        foreach (ItemSlotPanel slotPanel in slotPanels)
        {
            if(i < count){
                int index = ((activeSlotIndex + slotPanel.SlotIndex)%count + count)%count;
                if (0 <= index && index < currentItems.Count)
                {
                    slotPanel.SetItem(currentItems[index]);
                }
                else
                {
                    slotPanel.ClearSlot();
                }
            }
            else
            {
                slotPanel.ClearSlot();
            }
            i += 1;
            
        }
    }

    private void ClearSlotDisplay()
    {
        foreach (ItemSlotPanel slotPanel in slotPanels)
        {
            slotPanel.ClearSlot();
        }
    }

    public ItemData GetActiveItemData()
    {
        if(InventoryManager.instance.Items.Count <= 0) return null;
        return InventoryManager.instance.Items[activeSlotIndex].ItemData;
    }
}
