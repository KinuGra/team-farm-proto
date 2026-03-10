using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private GameObject nearItem = null;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            nearItem = other.gameObject;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            nearItem = null;
        }
    }
    
    void Update()
    {
        if (nearItem != null && Input.GetKeyDown(KeyCode.E))
        {
            Destroy(nearItem);
            nearItem = null;
        }
    }
}