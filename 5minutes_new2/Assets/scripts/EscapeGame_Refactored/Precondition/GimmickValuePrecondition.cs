using EscapeGame.Interaction;
using EscapeGame.SaveSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "EscapeGame/Preconditions/Gimmick Value", fileName = "Cond_Gimmick_")]
public sealed class GimmickValuePrecondition : InteractionPrecondition
{
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickDefinition _gimmick;
    [SerializeField] private int _expectedValue = 1;
    [SerializeField] private CompareMode _mode = CompareMode.Equals;

    public enum CompareMode
    {
        Equals,              // 値が一致
        GreaterThanOrEqual,  // 値以上
        LessThan,            // 値未満
    }

    public override bool Evaluate(in InteractionContext context)
    {
        if (_saveStore == null) return false;
        if (_gimmick == null) return false;
        if (string.IsNullOrEmpty(_gimmick.Id)) return false;

        int actual = _saveStore.ReadSaveData.GetGimmickValue(_gimmick.Id);

        return _mode switch
        {
            CompareMode.Equals => actual == _expectedValue,
            CompareMode.GreaterThanOrEqual => actual >= _expectedValue,
            CompareMode.LessThan => actual < _expectedValue,
            _ => false,
        };
    }
}