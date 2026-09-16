using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager instance;
    public bool isGameOverEnabled { get; private set; } = true;
    public bool isGameOver { get; private set; } = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public void GameOver()
    {
        if (!isGameOverEnabled) return; // ゲームオーバー可能か
        if (isGameOver) return; // すでにゲームオーバー
        isGameOver = true;

        Debug.Log("=== GAME OVER ===");

        // プレイヤーの動きを止める(✡重くなりそう)
        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.enabled = false;
        }

        // 3秒後にリスタート
        Invoke("RestartScene", 3f);
    }

    void RestartScene()
    {
        SetGameOverEnabled(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetGameOverEnabled(bool enabled)
    {
        isGameOverEnabled = enabled;
    }
}