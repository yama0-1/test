using EscapeGame.SaveSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

public sealed class ConditionMatchTrigger : MonoBehaviour
{
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickValuePrecondition[] _conditions;

    [SerializeField] private InteractEffect[] _effects;
    private bool _hasFired;
    private void OnEnable()
    {
        _saveStore.Changed += OnGimmickChanged;
        OnGimmickChanged(default,default);  
    }
    private void OnDisable()
        => _saveStore.Changed -= OnGimmickChanged;

    private void OnGimmickChanged(GimmickDefinition gimmick,int value)
    {
        if (_hasFired) return;

        foreach (var c in _conditions)
        {
            if (c == null) continue;
            if (!c.Evaluate(default)) return; 

        }

        _hasFired = true;

        foreach (var effect in _effects)
        {
            if (effect != null) effect.Execute(default);
        }

    }
}