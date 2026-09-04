using System;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeGame.SaveSystem
{
    [Serializable]
    public sealed class SaveData:IReadOnlySaveData
    {
        [SerializeField] private List<GimmickValueRecord> gimmickValues = new();

        public const int SlotCount = 8;

        [NonSerialized] private Dictionary<string, int> _cache;

        [SerializeField] private string[] _slotItemsID = new string[SlotCount];
        [SerializeField] private List<string> _obtainedItemIds = new();
        private void RebuildCache()
        {
            _cache = new Dictionary<string, int>(gimmickValues.Count);
            foreach (var r in gimmickValues) _cache[r.id] = r.value;
        }
        private void EnsureCache() { if (_cache == null) RebuildCache(); }


        public void SetSlot(int index, string id) => _slotItemsID[index] = id;
       

        public bool ContainsObtainedItem(string id) => _obtainedItemIds.Contains(id);

        public bool TryAddObtained(string id)
        {
            if (_obtainedItemIds.Contains(id)) return false;
            _obtainedItemIds.Add(id);
            return true;
        }
        public string GetSlotItemsID(int index) => _slotItemsID[index];
        public int GetGimmickValue(string id)
        {
            EnsureCache();
            return _cache.TryGetValue(id, out var v) ? v : 0;
        }

        public void SetGimmickValue(string id, int value)
        {
            EnsureCache();
            _cache[id] = value;

            for (int i = 0; i < gimmickValues.Count; i++)
                if (gimmickValues[i].id == id) { gimmickValues[i].value = value; return; }
            gimmickValues.Add(new GimmickValueRecord { id = id, value = value });
        }
    }

    [Serializable]
    public sealed class GimmickValueRecord
    {
        public string id;
        public int value;
    }

    public interface IReadOnlySaveData
    {
        int GetGimmickValue(string id);
        string GetSlotItemsID(int index);
        bool ContainsObtainedItem(string id);
    }
}