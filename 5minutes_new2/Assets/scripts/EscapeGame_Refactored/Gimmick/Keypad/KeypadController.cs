using EscapeGame.SaveSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 汎用キーパッドコントローラー
/// ScriptableObject で設定を外部化し、インスペクターで柔軟に設定可能
/// </summary>
public sealed class KeypadController : MonoBehaviour
{
    [Header("必須設定")]
    [SerializeField] private KeypadDefinition _definition;
    [SerializeField] private SaveStore _saveStore;
    
    [Header("表示")]
    [SerializeField] private TextMeshPro _display;
    
    [Header("イベント")]
    [SerializeField] private UnityEvent _onCorrect;
    [SerializeField] private UnityEvent _onWrong;
    [SerializeField] private UnityEvent _onInputChanged;
    
    private List<int> _input = new();
    private bool _isSolved = false;

    private void OnEnable()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// 数字を入力（0-9）
    /// </summary>
    public void InputDigit(int digit)
    {
        if (_isSolved) return;
        if (_input.Count >= _definition.MaxDigits) return;
        
        _input.Add(digit);
        UpdateDisplay();
        _onInputChanged?.Invoke();
    }

    /// <summary>
    /// 最後の入力を削除
    /// </summary>
    public void Delete()
    {
        if (_isSolved) return;
        if (_input.Count > 0)
        {
            _input.RemoveAt(_input.Count - 1);
            UpdateDisplay();
            _onInputChanged?.Invoke();
        }
    }

    /// <summary>
    /// 入力をすべてクリア
    /// </summary>
    public void Clear()
    {
        if (_isSolved) return;
        _input.Clear();
        UpdateDisplay();
        _onInputChanged?.Invoke();
    }

    /// <summary>
    /// 入力確定
    /// </summary>
    public void Confirm()
    {
        if (_isSolved) return;
        
        if (ValidateInput())
        {
            _isSolved = true;
            
            // ギミックの解決状態を保存
            if (_saveStore != null && _definition.SolvedGimmick != null)
            {
                _saveStore.SetGimmickValue(_definition.SolvedGimmick, _definition.SolvedGimmickIndex);
            }
            
            _onCorrect?.Invoke();
        }
        else
        {
            _onWrong?.Invoke();
            // 間違えた場合は入力をクリア（オプション）
            Clear();
        }
    }

    /// <summary>
    /// 入力が正解と一致するか検証
    /// </summary>
    private bool ValidateInput()
    {
        if (_definition == null) return false;
        return _definition.ValidateCode(_input.ToArray());
    }

    /// <summary>
    /// 表示を更新
    /// </summary>
    private void UpdateDisplay()
    {
        if (_display == null) return;

        if (_input.Count == 0)
        {
            _display.text = "";
            return;
        }

        if (_definition.ShowInput)
        {
            // 入力された数字を表示
            _display.text = string.Join("", _input);
        }
        else
        {
            // マスク表示（***）
            _display.text = new string(_definition.MaskChar, _input.Count);
        }
    }

    /// <summary>
    /// 現在の入力状態を取得（デバッグ・テスト用）
    /// </summary>
    public int[] GetCurrentInput() => _input.ToArray();

    /// <summary>
    /// 解決済みかどうかを取得
    /// </summary>
    public bool IsSolved => _isSolved;

    /// <summary>
    /// 手動で解決状態を設定（イベント発火なし）
    /// </summary>
    public void SetSolved(bool solved)
    {
        _isSolved = solved;
    }
}
