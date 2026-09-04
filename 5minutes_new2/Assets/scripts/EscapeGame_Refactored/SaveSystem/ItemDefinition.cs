using UnityEngine;

namespace EscapeGame.SaveSystem
{
    /// <summary>
    /// 1 アイテムの静的データ (表示名・アイコンなど)。
    /// 旧 Data クラス相当だが、視点座標などゲーム進行と無関係な情報は持たない。
    /// ScriptableObject なので Project ウィンドウでアセットとして作成・編集できる。
    /// </summary>
    [CreateAssetMenu(menuName = "EscapeGame/Item Definition", fileName = "Item_")]
    public sealed class ItemDefinition : ScriptableObject
    {
        [SerializeField] Sprite _icon;

        public string ID => name;
        public Sprite Icon => _icon;
    }
}
