using EscapeGame.Viewport;
using System.Collections.Generic;
using UnityEngine;
using System;
public sealed class ViewDirector : MonoBehaviour
{
    public ViewpointSO CurrentView { get; private set; }
    public bool CanGoBack => _historyView.Count > 0;
    public event Action ViewChanged;

    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private ViewportRequestChannel _requestChannel;
    [SerializeField] private ViewpointSO[] _initialViews = new ViewpointSO[4];

    private readonly Stack<ViewpointSO> _historyView = new();
    private bool CanMove => !_cameraMover.IsAnimating;

    private void OnEnable() => _requestChannel.Requested += HandleRequest;
    private void OnDisable() => _requestChannel.Requested -= HandleRequest;
    public bool IsCurrentViewInitial => IsInitialView(CurrentView);

    private void Start()
    {
        CurrentView = _initialViews[0];
        ViewChanged?.Invoke();
    }
    public void GoTo(ViewpointSO target)
    {
        if (!CanMove || target == CurrentView) return;
        UpdateHistory(target);
        SetView(target);
    }

    public void GoBack()
    {
        if (!CanMove || _historyView.Count <= 0) return;
        SetView(_historyView.Pop());
    }

    public void RotateRight() => Rotate(1);
    public void RotateLeft() => Rotate(-1);

    private void HandleRequest(ViewpointSO target) => GoTo(target);

    private void Rotate(int direction)
    {
        int current = GetInitialViewIndex(CurrentView);
        if (current < 0) return;
        int next = (current + direction + _initialViews.Length) % _initialViews.Length;
        GoTo(_initialViews[next]);
    }

    private void UpdateHistory(ViewpointSO target)
    {
        if (IsInitialView(target))
            _historyView.Clear();
        else
            _historyView.Push(CurrentView);
    }

    private void SetView(ViewpointSO target)
    {
        _cameraMover.MoveCamera(target);
        CurrentView = target;
        ViewChanged?.Invoke();
    }

    private int GetInitialViewIndex(ViewpointSO view)
    {
        for (int i = 0; i < _initialViews.Length; i++)
        {
            if (_initialViews[i] == view) return i;
        }
        return -1;
    }

    private bool IsInitialView(ViewpointSO view)
        => GetInitialViewIndex(view) >= 0;
}