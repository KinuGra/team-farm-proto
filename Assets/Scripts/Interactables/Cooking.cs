using UnityEngine;

public class CookingTable : MonoBehaviour
{
    public float interactDistance = 2f;

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

        bool hasRice = Inventory.instance.GetItemCount("CookedRice") > 0;
        bool hasWakame = Inventory.instance.GetItemCount("CookedWakame") > 0;

        bool canCraft = isNear && hasRice && hasWakame;

        highlight.SetHighlight(canCraft);

        if (canCraft && Input.GetKeyDown(KeyCode.F))
        {
            TryCraft();
        }
    }

    void TryCraft()
    {
        Inventory.instance.UseItem("CookedRice", 1);
        Inventory.instance.UseItem("CookedWakame", 1);
        Inventory.instance.AddItem("Onigiri", 1);

        Debug.Log("🍙 おにぎり作った！");
    }
}
