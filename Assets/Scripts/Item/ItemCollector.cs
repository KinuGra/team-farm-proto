using UnityEngine;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour
{
    private List<CollectableItem> itemsInRange = new List<CollectableItem>();
    private CollectableItem currentHighlight;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCollect();
        }

        UpdateHighlight();
    }

    void UpdateHighlight()
    {
        itemsInRange.RemoveAll(item => item == null);

        float closestDist = float.MaxValue;
        CollectableItem closest = null;

        foreach (CollectableItem item in itemsInRange)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = item;
            }
        }

        if (currentHighlight != null && currentHighlight != closest)
        {
            var oldHi = currentHighlight.GetComponent<HighlightController>();
            if (oldHi != null) oldHi.SetHighlight(false);
        }

        if (closest != null)
        {
            var newHi = closest.GetComponent<HighlightController>();
            if (newHi != null) newHi.SetHighlight(true);
        }

        currentHighlight = closest;
    }

    void TryCollect()
    {
        if (currentHighlight == null) return;

        Inventory.instance.AddItem(currentHighlight.itemName, currentHighlight.amount);
        Destroy(currentHighlight.gameObject);
        currentHighlight = null;
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<CollectableItem>();
        if (item != null)
        {
            itemsInRange.Add(item);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var item = other.GetComponent<CollectableItem>();
        if (item != null)
        {
            itemsInRange.Remove(item);
        }
    }
}