using System;
using EscapeGame.SaveSystem;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;


public sealed class InventoryModel
{
    public InventoryModel(SaveStore saveStore, ItemRegistry registry )
    {
        _saveStore = saveStore;
        _itemRegistry = registry;
    }

    private SaveStore _saveStore;
    private ItemRegistry _itemRegistry;
    /// <summary>スロット内容が変わったとき (index)。</summary>
    public event Action SlotChanged;
    /// <summary>選択スロットが変わったとき (newIndex, oldIndex)。-1 は未選択。</summary>
    public event Action SelectionChanged;

    public event Action<int> SlotReselected;
    public int SelectedIndex { get; private set; } = -1;

    public ItemDefinition GetItem(int slotIndex)
    {
        string id = _saveStore.ReadSaveData.GetSlotItemsID(slotIndex);

        return _itemRegistry.Resolve(id);
    }

    public ItemDefinition SelectedItem =>
        SelectedIndex >= 0 ? _itemRegistry.Resolve( _saveStore.ReadSaveData.GetSlotItemsID(SelectedIndex)) : null;

    /// <summary>最初の空きスロットにアイテムを入れる。満杯なら false。</summary>
    public bool TryAdd(ItemDefinition item)
    {
        if (item == null) return false;
        for (int i = 0; i < SaveData.SlotCount; i++)
        {
            if (!string.IsNullOrEmpty(_saveStore.ReadSaveData.GetSlotItemsID(i)))
                continue;

            _saveStore.SetSlot(i, item.ID);
            SlotChanged?.Invoke();

            _saveStore.MarkItemObtained(item.ID);
            return true;
        }
        return false;
    }

    /// <summary>選択中スロットの中身を別アイテムに差し替える</summary>
    public void ReplaceSelected(ItemDefinition newItem)
    {
        if (SelectedIndex < 0) return;
        _saveStore.SetSlot(SelectedIndex,newItem.ID);
        SlotChanged?.Invoke();
    }

    /// <summary>選択中スロットを空にする (アイテム消費)。</summary>
    public void ConsumeSelected()
    {
        if (SelectedIndex < 0) return;
        _saveStore.SetSlot(SelectedIndex,null);
        SelectedIndex = -1;
        SlotChanged?.Invoke();
    }

    /// <summary>スロットを選択する。空スロットは無視。</summary>
    public void Select(int index)
    {
        if (string.IsNullOrEmpty(_saveStore.ReadSaveData.GetSlotItemsID(index))) return;


        if (SelectedIndex == index)
        {
            SlotReselected?.Invoke(index); 
            return;
        }

        int previous = SelectedIndex;
        SelectedIndex = index;
        SelectionChanged?.Invoke();
    }

}