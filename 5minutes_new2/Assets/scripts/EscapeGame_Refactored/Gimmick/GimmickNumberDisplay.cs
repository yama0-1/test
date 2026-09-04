using EscapeGame.SaveSystem;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public sealed class GimmickNumberDisplay : MonoBehaviour
{
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickDefinition _watchGimmickValue;
    [SerializeField] private TextMeshPro _text;

    private void OnEnable()
    {
        _saveStore.Changed += UpdateVisual;
        UpdateVisual(default,default);
    }
    private void OnDisable()
        => _saveStore.Changed -= UpdateVisual;

    private void UpdateVisual(GimmickDefinition gimmick,int value)
    {
        int v = _saveStore.ReadSaveData.GetGimmickValue(_watchGimmickValue.Id);
        _text.text = v.ToString();
    }
}