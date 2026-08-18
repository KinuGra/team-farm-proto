using UnityEngine;

public class CookingTable : MonoBehaviour
{
    public float interactDistance = 2f;

    [SerializeField] private ItemData cookedRiceItem;
    [SerializeField] private ItemData cookedWakameItem;
    [SerializeField] private ItemData onigiriItem;

    private Transform player;
    private HighlightController highlight;

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

        float dist = Vector3.Distance(transform.position, player.position);
        bool isNear = dist <= interactDistance;

        bool hasRice = cookedRiceItem != null && InventoryManager.instance.GetItemCount(cookedRiceItem) > 0;
        bool hasWakame = cookedWakameItem != null && InventoryManager.instance.GetItemCount(cookedWakameItem) > 0;

        bool canCraft = isNear && hasRice && hasWakame;

        highlight.SetHighlight(canCraft);

        if (canCraft && Input.GetKeyDown(KeyCode.F))
        {
            TryCraft();
        }
    }

    void TryCraft()
    {
        if (cookedRiceItem == null || cookedWakameItem == null || onigiriItem == null) return;

        if (InventoryManager.instance.UseItem(cookedRiceItem, 1) &&
            InventoryManager.instance.UseItem(cookedWakameItem, 1))
        {
            InventoryManager.instance.AddItem(onigiriItem, 1);
            Debug.Log("🍙 おにぎり作った！");
        }
    }
}
