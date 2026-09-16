using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AutoSceneTrigger : MonoBehaviour
{
    [Header("遷移設定")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private bool useFadeEffect = true;
    

    private bool isPlayerInZone = false;

    private void Update()
    {
        // 領域内にいて、指定キーが押されたら
        if (isPlayerInZone)
        {
            isPlayerInZone = false;
            var request = new SceneTransitionRequest 
            { 
                TargetSceneName = targetSceneName 
            };

            // 特殊な演出をしたい場合は、ここで1回限りの戦略をリクエストに込める
            if (useFadeEffect)
            {
                request.CustomEffect = new FadeEffect(1.0f);
            }

            SceneTransitionManager.Instance.TransitionTo(request);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
        }
    }
}