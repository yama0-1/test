using EscapeGame.SaveSystem;
using UnityEngine;
using UnityEngine.Events;

public sealed class TimerController : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float _duration = 60f;

    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickValuePrecondition[] _runningCondition;
    [SerializeField] private GimmickDefinition _finishedGimmick;
    [SerializeField] private InteractEffect[] _effects;

    private const int _finishedValue = 1;

    private float _remaining;
    private bool _isFinished;

    private void OnEnable()
    {
        _remaining = _duration;

        _isFinished = _saveStore.ReadSaveData.GetGimmickValue(_finishedGimmick.Id) >= _finishedValue;
    }



    private void Update()
    {
        if (_isFinished) return;
        
        foreach(var condition in _runningCondition)
        {
            if (condition == null || !condition.Evaluate(default)) return;
        }

        _remaining -= Time.deltaTime;
        if (_remaining <= 0f)
        {
            _isFinished = true;
            
            _saveStore.SetGimmickValue(_finishedGimmick, _finishedValue);

            foreach (var effect in _effects)
            {
                effect.Execute(default);
            }


        }
    }
}