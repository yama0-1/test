using System;
using System.IO;
using UnityEngine;

namespace EscapeGame.SaveSystem
{
    [CreateAssetMenu(menuName = "EscapeGame/Save Store", fileName = "SaveStore")]
    public sealed class SaveStore : ScriptableObject
    {
        public event Action<GimmickDefinition , int> Changed;
        [NonSerialized] private  SaveData _data;

        public IReadOnlySaveData ReadSaveData
        {
            get
            {
                if (_data == null) Load();
                return _data;
            }
        }
        private static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");

        public void Load()
        {
            if (!File.Exists(FilePath))
            {
                _data = new SaveData();
                Save();
                return;
            }

            try
            {
                var json = File.ReadAllText(FilePath);
                _data = JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveStore] Load failed, using defaults. {e.Message}");
                _data = new SaveData();
            }
        }

        public void Save()
        {
            try
            {
                var json = JsonUtility.ToJson(_data);
                File.WriteAllText(FilePath, json);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveStore] Save failed. {e.Message}");
            }
        }

        public void ResetToNewGame()
        {
            _data = new SaveData();
            Save();
            Changed?.Invoke(default,default);
        }

        public void SetGimmickValue(GimmickDefinition gimmick, int value, bool persist = true)
        {
            _data.SetGimmickValue(gimmick.Id, value);
            Changed?.Invoke(gimmick,value);
            NotifyChanged(persist);
        }
        public void SetSlot(int index, string itemId)
        {
            _data.SetSlot(index, itemId);
            NotifyChanged();
        }
        public void MarkItemObtained(string itemId)
        {
            if (string.IsNullOrEmpty(itemId)) return;
            if(_data.TryAddObtained(itemId)) NotifyChanged();
        }

        public void NotifyChanged(bool persist = true)
        {
            if (persist) Save();
        }

    }

}
