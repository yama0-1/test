using EscapeGame.Viewport;
using UnityEngine;
using UnityEngine.UI;

public sealed class NavigationUI : MonoBehaviour
{
    [SerializeField] private ViewDirector _viewDirector;

    [Header("戻るボタン")]
    [SerializeField] private GameObject _backButton;

    [Header("横矢印")]
    [SerializeField] private GameObject _leftArrow;
    [SerializeField] private GameObject _rightArrow;

    private void OnEnable()
    {
        _viewDirector.ViewChanged += UpdateVisibility;
        UpdateVisibility();
    }

    private void OnDisable()
        => _viewDirector.ViewChanged -= UpdateVisibility;

    private void UpdateVisibility()
    {
        bool isInitial = _viewDirector.IsCurrentViewInitial;
        bool canGoBack = _viewDirector.CanGoBack;

        // 横矢印：初期ビューのときだけ
        _leftArrow.SetActive(isInitial);
        _rightArrow.SetActive(isInitial);

        // 戻る：初期ビュー以外のときだけ
        _backButton.SetActive(canGoBack);
    }
}