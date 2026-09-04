using EscapeGame.SaveSystem;
using UnityEngine;

public sealed class AnimatorEffect : InteractEffect
{
    public enum ParamType { Trigger, Int }

    [SerializeField] private Animator _animator;
    [SerializeField] private ParamType _type = ParamType.Trigger;
    [SerializeField] private string _paramName;
    [SerializeField] private SaveStore _saveStore; 
    [SerializeField] private GimmickDefinition _animateGimmick;

    // ロード時の復元

    private void Start()
    {
        if (_type != ParamType.Int) return;
        if (_animator == null || _saveStore == null || _animateGimmick == null) return;
        Execute(default);
    }

    public override void Execute(in InteractionContext context)
    {
        if (_animator == null) return;

        switch (_type)
        {
            case ParamType.Trigger:
                _animator.Play(_paramName, 0, 0f);
                break;
            case ParamType.Int:
                int value = _saveStore.ReadSaveData.GetGimmickValue(_animateGimmick.Id);

                _animator.SetInteger(_paramName, value);
                break;
        }
    }
}