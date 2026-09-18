using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UIを操作するために必要

// MonoBehaviourを継承しない純粋なC#クラス
public class FadeEffect : ITransitionEffect
{
    private float fadeTime;
    private GameObject fadeCanvasObj;
    private Image fadeImage;

    // コンストラクタ（フェード時間を指定。デフォルトは1秒）
    public FadeEffect(float fadeTime = 1.0f)
    {
        this.fadeTime = fadeTime;
    }

    // フェード用の黒い画面（CanvasとImage）を動的に生成する
    private void SetupUI()
    {
        if (fadeCanvasObj != null) return;

        // ① 最前面に表示されるCanvasを作成
        fadeCanvasObj = new GameObject("TransitionFadeCanvas");
        Object.DontDestroyOnLoad(fadeCanvasObj); // シーン遷移中も破棄されないようにする

        var canvas = fadeCanvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999; // UIの中で一番手前に表示

        // ② 黒い画像（Image）を作成してCanvasの子にする
        var imageObj = new GameObject("FadeImage");
        imageObj.transform.SetParent(fadeCanvasObj.transform, false);

        fadeImage = imageObj.AddComponent<Image>();
        fadeImage.color = new Color(0, 0, 0, 0); // 初期状態は「透明な黒」

        // ③ 画像を画面全体に広げる
        var rectTransform = fadeImage.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.sizeDelta = Vector2.zero;
    }

    // 画面を徐々に暗くする（PlayOut）
    public IEnumerator PlayOut()
    {
        PlayerController player = UnityEngine.Object.FindAnyObjectByType<PlayerController>();
        if(GameOverManager.instance != null)
        {
            Debug.Log("ゲームオーバーを無効化");
            GameOverManager.instance.SetGameOverEnabled(false); // ゲームオーバーを無効化
        }
        if (player != null)
        {
            player.enabled = false;
        }
        SetupUI(); // UIを生成
        
        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeTime);
            fadeImage.color = new Color(0, 0, 0, alpha); // アルファ値（不透明度）を上げる
            yield return null; // 1フレーム待つ
        }
        fadeImage.color = new Color(0, 0, 0, 1); // 確実に真っ黒にする
    }

    // 画面を徐々に明るくする（PlayIn）
    public IEnumerator PlayIn()
    {
        if (fadeImage == null) yield break;

        float timer = 0f;
        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / fadeTime);
            fadeImage.color = new Color(0, 0, 0, alpha); // アルファ値を下げる
            yield return null;
        }
        if(GameOverManager.instance != null)
        {
            Debug.Log("ゲームオーバーを有効化");
            GameOverManager.instance.SetGameOverEnabled(true); // ゲームオーバーを有効化
        }
        // 処理が終わったら、生成したCanvasごと綺麗に削除（ゴミを残さない）
        Object.Destroy(fadeCanvasObj);
    }
}