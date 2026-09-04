using EscapeGame.SaveSystem;
using UnityEngine;

public sealed class GimmickSpriteDisplay : MonoBehaviour
{
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickDefinition _watchGimmick;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Sprite[] _sprites; 

    private void OnEnable()
    {
        _saveStore.Changed += UpdateVisual;
        UpdateVisual(_watchGimmick, _saveStore.ReadSaveData.GetGimmickValue(_watchGimmick.Id));
    }

    private void OnDisable()
        => _saveStore.Changed -= UpdateVisual;

    private void UpdateVisual(GimmickDefinition gimmick, int value)
    {
        if (gimmick != _watchGimmick) return;
        if (value >= 0 && value < _sprites.Length)
            _renderer.sprite = _sprites[value];
    }
}