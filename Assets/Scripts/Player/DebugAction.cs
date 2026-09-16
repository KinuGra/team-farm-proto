using UnityEngine;

public class DebugAction : MonoBehaviour
{
    [SerializeField] private ItemData testItem;

    void Start()
    {

    }
    void Update()
    {
        // Pキーでおにぎり取得
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (testItem != null)
            {
                InventoryManager.instance.AddItem(testItem);
            }
        }
    }
}