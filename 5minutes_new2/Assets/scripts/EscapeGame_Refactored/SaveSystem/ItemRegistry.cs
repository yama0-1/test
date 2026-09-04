// ItemDefinition に [SerializeField] string _id を追加した前提
using EscapeGame.SaveSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EscapeGame/Registry/Item")]
public sealed class ItemRegistry : ScriptableObject
{
    [SerializeField] private ItemDefinition[] _items;


    public ItemDefinition Resolve(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        foreach (var it in _items) if (it.ID == id) return it;
        return null;
    }

    private void OnValidate()
    {
        var unique = new List<ItemDefinition>();
        var seen = new HashSet<string>();
        int removed = 0;
        foreach (var item in _items)
        {
            if (item == null) { removed++;  continue; }
            if (seen.Add(item.ID))
                unique.Add(item);
            else
                removed++;
             
        }

        if (removed > 0)
        {
            _items = unique.ToArray();
            Debug.Log($"ItemRegistry: 重複/null {removed} 件を削除", this);
        }
            
    }
}