using EscapeGame.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpEffect : InteractEffect
{
    [SerializeField] private ItemDefinition _item;
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private InventoryManager _inventory;

    private void Start()
    {
        if (_saveStore.ReadSaveData.ContainsObtainedItem(_item.ID)) gameObject.SetActive(false);
    }
    public override void Execute(in InteractionContext context)
    {
        Debug.Log("Interact:pickUP Item:"+_item);
        if (!_inventory.Model.TryAdd(_item)) return;
        gameObject.SetActive(false);
    }
}
