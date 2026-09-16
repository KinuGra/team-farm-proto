using UnityEngine;

public class ButtonPressSceneTrigger : MonoBehaviour
{
    [Header("遷移設定")]
    [SerializeField] private string targetSceneName;
    [SerializeField] private bool useFadeEffect = true;


    /// <summary>
    /// ボタンによるシーン遷移. ボタン押下時の処理に割り当てる
    /// </summary>
    public void OnSubmitButtonClicked()
    {
        var request = new SceneTransitionRequest 
        { 
            TargetSceneName = targetSceneName 
        };

        // 特殊な演出をしたい場合は、ここで1回限りの戦略をリクエストに込める
        if (useFadeEffect)
        {
            request.CustomEffect = new FadeEffect(1.5f);
        }
        SceneTransitionManager.Instance.TransitionTo(request);
    }
}