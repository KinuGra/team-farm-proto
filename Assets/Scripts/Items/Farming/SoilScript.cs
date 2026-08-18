using UnityEngine;

public class SoilScript : MonoBehaviour
{
    public GameObject cropPrefab;   // 作物のPrefab

    bool playerNear = false;        // プレイヤーが近くにいるか
    GameObject currentCrop;         // 今植えている作物

    int state = 0;                  // 畑の状態
    // 0 = 空
    // 1 = 成長中
    // 2 = 収穫できる

    void Update()
    {
        if(playerNear && Input.GetKeyDown(KeyCode.Space))
        {
            if(state == 0)
            {
                Plant();
            }
        }
    }

    void Plant()
    {
        currentCrop = Instantiate(
            cropPrefab,
            transform.position + Vector3.up * 0.5f,
            Quaternion.identity
        );

        state = 1;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}
