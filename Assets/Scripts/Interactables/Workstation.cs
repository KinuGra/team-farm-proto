using UnityEngine;

/// <summary>
/// 汎用作業台の基底クラス
/// Transaction配列を持ち、インタラクション時にトランザクションを実行
/// 調理台、取引所、その他作業台が継承可能
/// </summary>
public class Workstation : MonoBehaviour
{
    [SerializeField] protected float interactDistance = 2f;
    [SerializeField] protected Transaction[] transactions;

    protected Transform player;
    protected HighlightController highlight;

    protected virtual void Start()
    {
        highlight = GetComponent<HighlightController>();

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null)
        {
            player = p.transform;
        }
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool isNear = distance <= interactDistance;
        bool canInteract = isNear && HasAnyTransaction();

        if (highlight != null)
        {
            highlight.SetHighlight(canInteract);
        }

        if (isNear && Input.GetKeyDown(KeyCode.F))
        {
            if(canInteract)
            {
                TryExecuteAnyTransaction();
            }
            else
            {
                OnTransactionUnavailable();
            }
        }
    }

    /// <summary>
    /// 実行可能なトランザクションがあるか確認
    /// </summary>
    protected virtual bool HasAnyTransaction()
    {
        if (transactions == null || transactions.Length == 0)
            return false;

        InventoryManager inventory = InventoryManager.instance;
        if (inventory == null) return false;

        foreach (var transaction in transactions)
        {
            if (transaction != null && transaction.CanExecute(inventory))
                return true;
        }

        return false;
    }

    /// <summary>
    /// 実行可能な最初のトランザクションを実行
    /// </summary>
    protected virtual void TryExecuteAnyTransaction()
    {
        InventoryManager inventory = InventoryManager.instance;
        if (inventory == null) return;

        foreach (var transaction in transactions)
        {
            if (transaction != null && transaction.Execute(inventory))
            {
                OnTransactionExecuted(transaction);
                return;
            }
        }
    }

    /// <summary>
    /// トランザクション成功時に呼ばれるコールバック（オーバーライド可能）
    /// </summary>
    protected virtual void OnTransactionExecuted(Transaction transaction)
    {
        // 効果音やアニメーションなどをここで実装
    }

    /// <summary>
    /// トランザクション実行に失敗した時に呼ばれるコールバック（オーバーライド可能）
    /// </summary>
    protected virtual void OnTransactionFailed(Transaction transaction)
    {
        // 効果音やアニメーションなどをここで実装
    }

    /// <summary>
    /// トランザクションが利用できない場合に呼ばれるコールバック（オーバーライド可能）
    /// </summary>
    protected virtual void OnTransactionUnavailable()
    {
        // 効果音やアニメーションなどをここで実装
    }
    
}
