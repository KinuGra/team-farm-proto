using UnityEngine;
using System.Collections.Generic;

public class Campfire : MonoBehaviour
{
    public float interactDistance = 2f;

    private Transform player;
    private HighlightController highlight;

    private Dictionary<string, string> recipes = new Dictionary<string, string>()
    {
        { "Rice", "CookedRice" },
        { "Wakame", "CookedWakame" }
    };

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
            if (Inventory.instance.GetItemCount(recipe.Key) > 0)
                return true;
        }
        return false;
    }

    void TryCook()
    {
        foreach (var recipe in recipes)
        {
            if (Inventory.instance.UseItem(recipe.Key, 1))
            {
                Inventory.instance.AddItem(recipe.Value);
                Debug.Log(recipe.Key + " → " + recipe.Value);
                return;
            }
        }
    }
}
