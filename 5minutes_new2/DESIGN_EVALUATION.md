# 脱出ゲーム設計評価と改善提案

## 現在の設計評価 (75/100点)

### ✅ 優れている点

#### 1. アーキテクチャ設計
- **Command/Strategy パターンの適切な適用**: `InteractionPrecondition` と `InteractEffect` の分離により、新規ギミック追加が容易
- **関心の分離**: Model/View/Controller の役割分担が明確（InventoryModel/View など）
- **イベント駆動アーキテクチャ**: `SaveStore.Changed` イベントで効率的な状態同期
- **依存関係の最小化**: `ViewportRequestChannel` で疎結合なコンポーネント間通信

#### 2. Unity 最適化
- **ScriptableObject の効果的な活用**: データ駆動設計でデザイナーも操作可能
- **Inspector 編集対応**: 多くの設定がインスペクターから変更可能
- **アセット管理**: `CreateAssetMenu` で整理されたメニュー構造

#### 3. 拡張性
- **Open/Closed 原則**: 新しい条件や効果は既存コードを変更せず追加可能
- **多態性の活用**: `InteractionPrecondition` 配列で複数条件の評価

### ⚠️ 改善が必要な点

#### 1. ディレクトリ構成の問題（重要）
**問題**: `test/` ディレクトリに本番コードが混在
```
Assets/scripts/EscapeGame_Refactored/test/
├── Interactable.cs              # 本番使用
├── InteractEffect.cs            # 本番使用
├── KeypadController.cs          # 旧版（ハードコードあり）
├── PickUpEffect.cs              # 本番使用
├── GimmickValueEffect.cs        # 本番使用
└── ... (他の Effect クラス)
```

**影響**: 
- 新旧コードの混在で混乱を招く
- どのコントローラーを使うべきか不明確
- リファクタリングの進行状況が把握しにくい

**解決策**:
```bash
# 推奨ディレクトリ構成
Assets/scripts/EscapeGame_Refactored/
├── Core/                        # 新規作成：基盤クラス
│   ├── Interactable.cs
│   ├── InteractEffect.cs
│   └── InteractionContext.cs
├── Gimmick/
│   ├── Keypad/                  # 新規：汎用キーパッド
│   │   ├── KeypadDefinition.cs
│   │   ├── KeypadController.cs
│   │   └── KeypadButtonEffect.cs
│   ├── GimmickSpriteDisplay.cs
│   └── ...
├── Effects/                     # 新規：Effect クラスを移動
│   ├── PickUpEffect.cs
│   ├── GimmickValueEffect.cs
│   ├── ViewPointEffect.cs
│   ├── ItemEffect.cs
│   └── ...
├── Precondition/
├── Inventory/
├── Viewport/
├── UI/
├── SaveSystem/
└── test/                        # 本当のテストのみ配置
    └── (ユニットテストファイル)
```

#### 2. Magic Number の多用
**問題箇所**:
```csharp
// 旧 KeypadController (test/)
private int[] _correctCode = {7,3,6,3};  // ハードコード
private int _maxDigits = 4;              // 固定値

// TimerController
private const int _finishedValue = 1;    // Magic number
```

**改善案**:
```csharp
// ScriptableObject で外部化（KeypadDefinition は完了済み）
// TimerController も同様に改善可能
[CreateAssetMenu(menuName = "EscapeGame/Gimmick/Timer Definition")]
public class TimerDefinition : ScriptableObject
{
    [SerializeField] private float _duration = 60f;
    [SerializeField] private int _finishedValue = 1;
    // ...
}
```

#### 3. null 安全性の課題
**問題**: 各所での null チェックが散在
```csharp
// 改善前
if (_saveStore == null) return false;
if (_gimmick == null) return false;
if (_animator == null) return;

// 改善案：早期リターンと Assert の活用
private void Awake()
{
    Debug.Assert(_saveStore != null, "SaveStore が設定されていません");
    Debug.Assert(_gimmick != null, "GimmickDefinition が設定されていません");
}
```

#### 4. SaveSystem の脆弱性
**問題**:
- JSON ファイルが平文で保存（改ざん容易）
- 破損時のフォールバックが単純
- バージョン管理なし

**改善案**:
```csharp
public sealed class SaveStore : ScriptableObject
{
    // 簡易チェックサム
    private string CalculateChecksum(string json)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        var hash = System.Security.Cryptography.SHA256.Create();
        var hashBytes = hash.ComputeHash(bytes);
        return System.Convert.ToBase64String(hashBytes);
    }
    
    public void Save()
    {
        try
        {
            var json = JsonUtility.ToJson(_data);
            var checksum = CalculateChecksum(json);
            File.WriteAllText(FilePath, json + "|" + checksum);
        }
        catch (Exception e)
        {
            Debug.LogError($"[SaveStore] Save failed. {e.Message}");
        }
    }
}
```

#### 5. Keypad の汎用性（改善済み✅）
**旧実装の問題**:
- ハードコードされた正解コード
- 固定の桁数
- 表示形式の固定

**新実装（完了）**:
- ✅ `KeypadDefinition` で設定外部化
- ✅ マスク表示対応
- ✅ 再利用性向上

---

## Inspector 操作ガイド

### 現在 Inspector から設定可能な項目

#### 1. KeypadController（汎用版）
```
Keypad オブジェクト
├── KeypadController
│   ├── Definition: KeypadDefinition アセット
│   ├── Save Store: SaveStore アセット
│   ├── Display: TextMeshPro
│   ├── On Correct: UnityEvent
│   └── On Wrong: UnityEvent
```

**手順**:
1. Project ウィンドウで右クリック → `Create > EscapeGame > Gimmick > Keypad Definition`
2. インスペクターで設定：
   - Correct Code: `[7, 3, 6, 3]`
   - Max Digits: `4`
   - Show Input: `true/false`
   - Solved Gimmick: GimmickDefinition
3. Keypad オブジェクトに KeypadController を追加
4. 各フィールドをドラッグ＆ドロップ

#### 2. Interactable（相互作用オブジェクト）
```
インタラクト可能オブジェクト
├── Interactable
│   ├── Conditions: InteractionPrecondition[]
│   │   ├── GimmickValuePrecondition
│   │   ├── SelectedItemPrecondition
│   │   └── CurrentViewPrecondition
│   └── Effects: InteractEffect[]
│       ├── PickUpEffect
│       ├── GimmickValueEffect
│       └── ViewPointEffect
```

**設定例**: 「鍵がかかったドア」
```
Conditions:
  - GimmickValuePrecondition
    - Save Store: SaveStore
    - Gimmick: "Door_Locked"
    - Expected Value: 0
    - Mode: Equals

Effects:
  - GimmickValueEffect
    - Save Store: SaveStore
    - Gimmick: "Door_Open"
    - Mode: Set
    - Set Value: 1
```

#### 3. Inventory システム
```
InventoryManager オブジェクト
├── InventoryManager
│   ├── Save Store: SaveStore
│   └── Registry: ItemRegistry

InventoryView オブジェクト
├── InventoryView
│   ├── Slot Icons: Image[4]
│   ├── Selection Frames: GameObject[4]
│   ├── Inventory: InventoryManager
│   ├── Prev Button: GameObject
│   ├── Next Button: GameObject
│   └── Item Zoom: GameObject
```

**アイテム登録手順**:
1. `Create > EscapeGame > Item Definition` でアイテム作成
2. アイコンスプライトを設定
3. `Create > EscapeGame > Registry > Item` で ItemRegistry 作成
4. 全 ItemDefinition を登録

#### 4. Viewport システム
```
ViewDirector オブジェクト
├── ViewDirector
│   ├── Camera Mover: CameraMover
│   ├── Request Channel: ViewportRequestChannel
│   └── Initial Views: ViewpointSO[4]

Camera オブジェクト
└── CameraMover
    ├── Duration: 0.2
    └── Easing: AnimationCurve
```

**視点設定手順**:
1. `Create > EscapeGame > Viewpoint` で ViewpointSO 作成
2. Camera Position と EulerAngles を設定
3. ViewDirector の Initial Views に登録

#### 5. Gimmick 連動表示
```
ギミック連動オブジェクト
├── GimmickSpriteDisplay
│   ├── Save Store: SaveStore
│   ├── Watch Gimmick: GimmickDefinition
│   ├── Renderer: SpriteRenderer
│   └── Sprites: Sprite[] (状態ごとの画像)
└── GimmickNumberDisplay
    ├── Save Store: SaveStore
    ├── Watch Gimmick Value: GimmickDefinition
    └── Text: TextMeshPro
```

---

## 優先度の高い改善タスク

### 優先度 高 🔴
1. **ディレクトリ構成の整理**
   - `test/` から本番コードを移動
   - `Core/` と `Effects/` ディレクトリを作成
   
2. **単体テストの導入**
   - NUnit/Unity Test Framework の設定
   - 主要ロジックのテストカバレッジ確保

3. **null チェックの統一**
   - `Debug.Assert` の導入
   - 早期リターンパターンの適用

### 優先度 中 🟡
4. **TimerController の ScriptableObject 化**
   - `TimerDefinition` の作成
   - Inspector で設定可能に

5. **SaveSystem の強化**
   - チェックサムによる改ざん検知
   - バージョン管理の導入

6. **エラーハンドリングの改善**
   - 一貫したログ出力
   - ユーザーフレンドリーなエラーメッセージ

### 優先度 低 🟢
7. **ドキュメント整備**
   - 各システムの使用方法
   - ギミック追加ガイド

8. **パフォーマンス最適化**
   - イベント購読の最適化
   - 不要な Update 処理の削除

---

## コード品質チェックリスト

### 実装前に確認
- [ ] 既存の `test/` コードを使っていないか？
- [ ] Magic Number を ScriptableObject で外部化できるか？
- [ ] null チェックは適切か？
- [ ] Inspector から設定可能か？
- [ ] デザイナーが調整可能なパラメータか？

### 実装後に確認
- [ ] ユニットテストが書ける構造か？
- [ ] ログ出力は十分か？
- [ ] メモリリークのリスクはないか（イベント購読解除）？
- [ ] ドキュメントは更新したか？

---

## まとめ

現在の設計は**データ駆動と関心の分離**という点で優れていますが、**ディレクトリ構成とテスト構造**に課題があります。

KeypadController の汎用化は完了したので、次に以下の対応を推奨します：

1. **即座に対応**: `test/` ディレクトリの整理
2. **短期対応**: 単体テスト環境の構築
3. **中長期対応**: SaveSystem の強化とドキュメント整備

これにより、保守性と拡張性がさらに向上し、デザイナーとエンジニアの協業もスムーズになります。
