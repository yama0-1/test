using EscapeGame.SaveSystem;
using UnityEngine;

public sealed class ItemEffect : InteractEffect
{
    public enum Mode { Consume, Transform, None }

    [SerializeField] private Mode _mode = Mode.Consume;
    [SerializeField] private ItemDefinition _transformTo;
    [SerializeField] private InventoryManager _inventory;

    public override void Execute(in InteractionContext context)
    {
        var selected = context.SelectedItem;
        if (selected == null) return;

        switch (_mode)
        {
            case Mode.Consume:
                _inventory.Model.ConsumeSelected();
                break;

            case Mode.Transform:
                _inventory.Model.ReplaceSelected(_transformTo);
                break;
        }
    }
}