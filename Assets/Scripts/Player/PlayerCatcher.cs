using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("[接触] クリーチャーに捕まった！");
            GameOverManager.instance.GameOver();
        }
    }
}