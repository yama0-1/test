using UnityEngine;

public sealed class KeypadButtonEffect : InteractEffect
{
    public enum ActionType { Digit, Delete, Clear, Confirm }

    [SerializeField] private KeypadController _controller;
    [SerializeField] private ActionType _action;
    [SerializeField] private int _digit;  // Digit 用（0〜9）

    public override void Execute(in InteractionContext context)
    {
        if (_controller == null) return;

        switch (_action)
        {
            case ActionType.Digit: _controller.InputDigit(_digit); break;
            case ActionType.Delete: _controller.Delete(); break;
            case ActionType.Clear: _controller.Clear(); break;
            case ActionType.Confirm: _controller.Confirm(); break;
        }
    }
}