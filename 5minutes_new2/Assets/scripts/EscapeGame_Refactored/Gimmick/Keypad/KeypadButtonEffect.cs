using UnityEngine;

/// <summary>
/// 汎用キーパッドボタンエフェクト
/// KeypadController を操作する InteractEffect
/// </summary>
public sealed class KeypadButtonEffect : InteractEffect
{
    public enum ActionType { Digit, Delete, Clear, Confirm }
    
    [SerializeField] private KeypadController _controller;
    [SerializeField] private ActionType _action;
    [SerializeField] private int _digit;  // Digit 用（0〜9）

    public override void Execute(in InteractionContext context)
    {
        if (_controller == null)
        {
            Debug.LogWarning("[KeypadButtonEffect] Controller が設定されていません");
            return;
        }

        switch (_action)
        {
            case ActionType.Digit:
                if (_digit >= 0 && _digit <= 9)
                    _controller.InputDigit(_digit);
                else
                    Debug.LogWarning($"[KeypadButtonEffect] 無効な数字：{_digit}");
                break;
            case ActionType.Delete:
                _controller.Delete();
                break;
            case ActionType.Clear:
                _controller.Clear();
                break;
            case ActionType.Confirm:
                _controller.Confirm();
                break;
        }
    }
}
