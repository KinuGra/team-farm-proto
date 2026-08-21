using UnityEngine;

/// <summary>
/// Workstation の具体例
/// Inspector で Transaction[] を設定するだけで動作する汎用作業台
/// 
/// 例：
/// - 調理台：Rice → CookedRice のトランザクション、Wakame → CookedWakame のトランザクションなど
/// - 取引所：複数アイテム → 報酬アイテム
/// </summary>
public class SimpleWorkstation : Workstation
{
    // 必要に応じてカスタマイズ可能
    // 基本的には Workstation の Start/Update で十分
}
