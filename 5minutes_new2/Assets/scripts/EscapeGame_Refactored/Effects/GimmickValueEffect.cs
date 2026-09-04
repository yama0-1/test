using EscapeGame.SaveSystem;
using UnityEngine;

public sealed class GimmickValueEffect : InteractEffect
{
    public enum Mode { Set, Toggle, Increment }

    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickDefinition _gimmick;
    [SerializeField] private Mode _mode = Mode.Set;
    [SerializeField] private int _setValue = 1;

    public override void Execute(in InteractionContext context)
    {
        int current = _saveStore.ReadSaveData.GetGimmickValue(_gimmick.Id);

        int newValue = _mode switch
        {
            Mode.Set => _setValue,
            Mode.Toggle => current == 0 ? 1 : 0,
            Mode.Increment => (current + 1) % _setValue,
            _ => current
        };

        _saveStore.SetGimmickValue(_gimmick, newValue);
    }
}
