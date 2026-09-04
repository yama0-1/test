using EscapeGame.SaveSystem;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public sealed class KeypadController : MonoBehaviour
{
    [SerializeField] private SaveStore _saveStore;
    [SerializeField] private GimmickDefinition _solvedGimmick;
    [SerializeField] private int _solvedGimmickIndex;
    private int[] _correctCode = {7,3,6,3};
    private int _maxDigits = 4;

    [Header("表示")]
    [SerializeField] private TextMeshPro _display;

    [SerializeField] private UnityEvent _onCorrect;
    [SerializeField] private UnityEvent _onWrong;

    private List<int> _input =new();

    private void OnEnable()
    {
        UpdateDisplay();
    }


    public void InputDigit(int digit)
    {
        if (_input.Count >= _maxDigits) return;
        _input.Add(digit);
        UpdateDisplay();
    }

    public void Delete()
    {
        if (_input.Count > 0)
            _input.RemoveAt(_input.Count - 1);
        UpdateDisplay();
    }

    public void Clear()
    {
        _input.Clear();
        UpdateDisplay();
    }

    public void Confirm()
    {
        if (IsMatch(_input))
        {
            _saveStore.SetGimmickValue(_solvedGimmick, _solvedGimmickIndex);
            _onCorrect?.Invoke();
        }
        else
        {
            _onWrong?.Invoke();
        }
    }
    private bool IsMatch(List<int> input)
    {
        for (int i = 0; i < input.Count; i++)
        {
            if (input[i] != _correctCode[i]) return false;
        }

        return true;
    }
    private void UpdateDisplay()
    {
        if (_display == null) return;

        string shown = _input.Count > 0
       ? string.Join("", _input)
       : "";

        _display.text = shown; 
    }
}