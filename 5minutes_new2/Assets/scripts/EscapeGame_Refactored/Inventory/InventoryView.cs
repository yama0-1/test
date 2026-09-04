using EscapeGame.SaveSystem;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace EscapeGame.Inventory
{
    /// <summary>
    /// InventoryModel と Unity UI (スロット画像・選択枠) を橋渡しする。
    /// ロジックは持たず、モデルのイベントを購読して見た目を更新するだけ。
    /// 旧 GameDirector の LoadAllItemSlot / slotsImage / slotsFrame 操作を集約。
    /// </summary>
    public sealed class InventoryView : MonoBehaviour
    {
        private const int SlotsPerPage = 4;
        private const int PageCount = 2;
        private int _page = 0;
        private int SlotIndex(int frame) => _page * SlotsPerPage + frame;

        // スロット番号が、今のページの何番目の枠か(映っていなければ -1)
        private int FrameIndex(int slotIndex)
        {
            if (slotIndex / SlotsPerPage != _page) return -1;  // 別ページなら映らない
            return slotIndex % SlotsPerPage;
        }

        [SerializeField] private Image[] _slotIcons = new Image[SlotsPerPage];
        [SerializeField] private GameObject[] _selectionFrames = new GameObject[SlotsPerPage];
        [SerializeField] private InventoryManager _inventory;
        [SerializeField] private GameObject _prevButton;
        [SerializeField] private GameObject _nextButton;
        [SerializeField] private GameObject _itemZoom;
        [SerializeField] private Image _itemZoomImage;
        private void OnEnable()
        {
            _inventory.Model.SlotChanged += UpdateVisual;
            _inventory.Model.SelectionChanged += UpdateVisual;
            _inventory.Model.SlotReselected += ZoomItem;
        }

        private void OnDisable()
        {
            _inventory.Model.SlotChanged -= UpdateVisual;
            _inventory.Model.SelectionChanged -= UpdateVisual;
            _inventory.Model.SlotReselected -= ZoomItem;
        }

        private void Start() => UpdateVisual();

        

        /// <summary>セーブ復帰時など、全スロットを描き直す。</summary>

        private void UpdateVisual()
        {
            _prevButton.SetActive(_page > 0);
            _nextButton.SetActive(_page < PageCount - 1);

            foreach (var frame in _selectionFrames) frame.SetActive(false);

            // 選択枠：選択中かつこのページに表示される場合だけ表示
            int selectedInd = _inventory.Model.SelectedIndex;
            if (selectedInd != -1)
            {
                int frame = FrameIndex(selectedInd);
                if (frame >= 0 && frame < _selectionFrames.Length)
                    _selectionFrames[frame].SetActive(true);
            }

            // スロットアイコン：このページ分だけ更新
            for (int i = 0; i < SlotsPerPage; i++)
            {
                int slotIndex = SlotIndex(i);
                var item = _inventory.Model.GetItem(slotIndex);
                _slotIcons[i].sprite = item != null ? item.Icon : null;
            }

        }
        private void ZoomItem(int index)
        {
            var item = _inventory.Model.GetItem(index);
            if (item == null) return;
            _itemZoomImage.sprite = item.Icon;
            _itemZoom.SetActive(true);
        }

        public void CloseZoom()
        {
            _itemZoom.SetActive(false); 
        }

        public void OnSlotButtonPressed(int frame)
        {
            int slotInd = SlotIndex(frame);
            _inventory.Model.Select(slotInd);
        }
        public void NextPage()
        {
            if (_page >= PageCount - 1) return;
            _page++;
            UpdateVisual();
        }

        public void PrevPage()
        {
            if (_page <= 0) return;
            _page--;
            UpdateVisual();
        }


    }
}
