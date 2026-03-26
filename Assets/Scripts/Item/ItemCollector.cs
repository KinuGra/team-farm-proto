using UnityEngine;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour
{
    private List<CollectableItem> itemsInRange = new List<CollectableItem>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryCollect();
        }
    }

    void TryCollect()
    {
        // リストを掃除（Destroyされたものを除去）
        itemsInRange.RemoveAll(item => item == null);

        if (itemsInRange.Count == 0) return;

        // 一番近いアイテムを回収
        CollectableItem closest = null;
        float closestDist = float.MaxValue;

        foreach (CollectableItem item in itemsInRange)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closest = item;
            }
        }

        if (closest != null)
        {
            Inventory.instance.AddItem(closest.itemName, closest.amount);
            Debug.Log(closest.itemName + " を回収！");
            Destroy(closest.gameObject);
            itemsInRange.Remove(closest);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        CollectableItem item = other.GetComponent<CollectableItem>();
        if (item != null)
        {
            itemsInRange.Add(item);
            Debug.Log(item.itemName + " が回収範囲内");
        }
    }

    void OnTriggerExit(Collider other)
    {
        CollectableItem item = other.GetComponent<CollectableItem>();
        if (item != null)
        {
            itemsInRange.Remove(item);
        }
    }
}