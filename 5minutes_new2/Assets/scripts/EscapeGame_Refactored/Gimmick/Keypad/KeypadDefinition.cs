using UnityEngine;

[CreateAssetMenu(menuName = "EscapeGame/Gimmick/Keypad Definition")]
public sealed class KeypadDefinition : ScriptableObject
{
    [Header("基本設定")]
    [SerializeField] private int[] _correctCode;
    [SerializeField] private int _maxDigits = 4;
    
    [Header("表示設定")]
    [SerializeField] private bool _showInput = true;  // true: 入力表示，false: マスク（***）
    [SerializeField] private char _maskChar = '*';
    
    [Header("ギミック連動")]
    [SerializeField] private GimmickDefinition _solvedGimmick;
    [SerializeField] private int _solvedGimmickIndex = 0;
    
    public int[] CorrectCode => _correctCode;
    public int MaxDigits => _maxDigits;
    public bool ShowInput => _showInput;
    public char MaskChar => _maskChar;
    public GimmickDefinition SolvedGimmick => _solvedGimmick;
    public int SolvedGimmickIndex => _solvedGimmickIndex;
    
    public bool ValidateCode(int[] input)
    {
        if (input == null || _correctCode == null) return false;
        if (input.Length != _correctCode.Length) return false;
        
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] != _correctCode[i]) return false;
        }
        return true;
    }
}
