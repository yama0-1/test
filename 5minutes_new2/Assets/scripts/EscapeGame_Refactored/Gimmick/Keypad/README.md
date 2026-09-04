# 汎用 Keypad システム

## 概要
ScriptableObject を使用して設定を外部化し、インスペクターから柔軟に設定可能なキーパッドシステムです。

## 改善点

### 旧実装の問題点
1. **ハードコードされた正解**: `_correctCode = {7,3,6,3}` が直接コードに埋め込まれていた
2. **固定の桁数**: `_maxDigits = 4` が変更できなかった
3. **表示形式の固定**: 入力値が常にそのまま表示されていた
4. **再利用性の低さ**: 新しいキーパッドを作るたびにコード修正が必要だった

### 新実装の機能

#### 1. KeypadDefinition (ScriptableObject)
正解コードや表示設定を ScriptableObject で管理：

```csharp
[CreateAssetMenu(menuName = "EscapeGame/Gimmick/Keypad Definition")]
public class KeypadDefinition : ScriptableObject
{
    // 基本設定
    [SerializeField] private int[] _correctCode;      // 正解のコード
    [SerializeField] private int _maxDigits = 4;      // 最大桁数
    
    // 表示設定
    [SerializeField] private bool _showInput = true;  // 入力表示の有無
    [SerializeField] private char _maskChar = '*';    // マスク文字
    
    // ギミック連動
    [SerializeField] private GimmickDefinition _solvedGimmick;
    [SerializeField] private int _solvedGimmickIndex;
}
```

#### 2. KeypadController
汎用的なキーパッド制御：

- ✅ **データ駆動設計**: KeypadDefinition から設定を読み込み
- ✅ **マスク表示対応**: パスワード入力時は `***` 表示も可能
- ✅ **イベント拡張**: `_onInputChanged` イベントを追加
- ✅ **状態管理**: `IsSolved` プロパティで解決済み状態を管理
- ✅ **null セーフティ**: 適切な null チェックと警告ログ

#### 3. KeypadButtonEffect
InteractEffect と統合してボタン操作を処理：

- デバッグログ追加で設定ミスを検出
- 数字の範囲チェック（0-9）

## 使用方法

### ステップ 1: KeypadDefinition の作成
1. Project ウィンドウで右クリック
2. `Create > EscapeGame > Gimmick > Keypad Definition`
3. インスペクターで設定：
   - Correct Code: `[7, 3, 6, 3]`
   - Max Digits: `4`
   - Show Input: `true` (または `false` でマスク表示)
   - Solved Gimmick: 該当する GimmickDefinition を設定

### ステップ 2: KeypadController の設定
1. キーパッドオブジェクトに `KeypadController` コンポーネントを追加
2. 以下のフィールドを設定：
   - **Definition**: ステップ 1で作成した KeypadDefinition
   - **Save Store**: SaveStore アセット
   - **Display**: TextMeshPro コンポーネント
   - **On Correct**: 正解時のイベント（ドア開閉など）
   - **On Wrong**: 不正解時のイベント（エラードアなど）

### ステップ 3: ボタンの設定
各ボタンオブジェクトに `KeypadButtonEffect` を追加：

```
ActionType: Digit
Digit: 7  (数字ボタン用)

ActionType: Delete  (削除ボタン用)
ActionType: Clear   (クリアボタン用)
ActionType: Confirm (確定ボタン用)
```

## コード例

### 異なるコードのキーパッドを複数作成
```
Keypad_Door1.asset:
  - Correct Code: [1, 2, 3, 4]
  
Keypad_Safe.asset:
  - Correct Code: [7, 3, 6, 3]
  
Keypad_Elevator.asset:
  - Correct Code: [5, 5, 5]
  - Max Digits: 3
```

### マスク表示を使う（パスワード入力）
```
Keypad_Password.asset:
  - Correct Code: [1, 9, 4, 5]
  - Show Input: false
  - Mask Char: '*'
```

## メリット

1. **デザイナーフレンドリー**: コード修正なしで正解を変更可能
2. **再利用性**: 同じコンポーネントで複数のキーパッドを作成可能
3. **保守性**: 設定が ScriptableObject に集約されており管理が容易
4. **拡張性**: 新しい表示形式や機能を追加しやすい
5. **デバッグ容易**: 警告ログで設定ミスを検出可能

## 旧ファイルからの移行

元の `test/KeypadController.cs` は `/test` ディレクトリに残していますが、新しい実装を使用するには：

1. 既存の Keypad オブジェクトの KeypadController を新しいものに置き換え
2. KeypadDefinition アセットを作成して設定を移行
3. Inspector で参照を設定し直し

## ファイル構成
```
Assets/scripts/EscapeGame_Refactored/Gimmick/Keypad/
├── KeypadDefinition.cs        # 設定用 ScriptableObject
├── KeypadController.cs        # コントローラー
└── KeypadButtonEffect.cs      # ボタンエフェクト
```
