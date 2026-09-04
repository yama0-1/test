using EscapeGame.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EscapeGame.Inventory;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private ItemRegistry _registry;
    private InventoryModel _model;

    public InventoryModel Model
    {
        get
        {
            if (_model == null)
                _model = new InventoryModel(_saveStore, _registry);
            return _model;
        }
    }
}
