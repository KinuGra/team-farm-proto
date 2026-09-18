using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンローダーのインターフェース
/// </summary>
public interface ISceneLoader
{
    /// <summary>
    /// シーン名を受け取って非同期でシーンをロードする処理
    /// </summary>
    IEnumerator LoadSceneAsync(string sceneName);
}
/// <summary>
/// シーン遷移のエフェクトのインターフェース
/// </summary>
public interface ITransitionEffect
{
    /// <summary>
    /// シーン遷移前のエフェクト
    /// </summary>
    IEnumerator PlayOut();

    /// <summary>
    /// シーン遷移後のエフェクト
    /// </summary> 
    IEnumerator PlayIn();
}


/// <summary>
/// シーン遷移のリクエストを表す構造体
/// </summary>
public struct SceneTransitionRequest
{
    public string TargetSceneName;
    public ISceneLoader CustomLoader;
    public ITransitionEffect CustomEffect;
}


/// <summary>
/// デフォルトのシーンロード処理
/// </summary>
public class DefaultSceneLoader : ISceneLoader
{
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        if (DynamicObjectManager.Instance != null)
        {
            DynamicObjectManager.Instance.SaveCurrentSceneObjects();
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}

/// <summary>
/// シーン遷移を管理するマネージャー(シングルトン)
/// </summary>
public class SceneTransitionManager : MonoBehaviour
{
    // シングルトンであることを保証するためのインスタンス
    public static SceneTransitionManager Instance { get; private set; }

    // Managerが保持するのは「デフォルトの戦略」のみ
    private ISceneLoader defaultLoader;

    private bool isTransitioning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // デフォルトのローダーを設定
            defaultLoader = new DefaultSceneLoader();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TransitionTo(SceneTransitionRequest request)
    {
        if (isTransitioning) return;
        StartCoroutine(TransitionSequence(request));
    }

    private IEnumerator TransitionSequence(SceneTransitionRequest request)
    {
        isTransitioning = true;

        // 要求にカスタム指定があればそれを使い、なければデフォルトを使用
        ISceneLoader activeLoader = request.CustomLoader ?? defaultLoader;
        ITransitionEffect activeEffect = request.CustomEffect; // nullなら演出なし

        // 決定した戦略を実行
        if (activeEffect != null) yield return activeEffect.PlayOut();
        
        yield return activeLoader.LoadSceneAsync(request.TargetSceneName);
        
        if (activeEffect != null) yield return activeEffect.PlayIn();

        isTransitioning = false;
    }
}