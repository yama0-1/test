using System;
using UnityEngine;

namespace EscapeGame.Viewport
{
    /// <summary>
    /// 「この視点へ移動して」という要求を中継する SO チャネル。
    /// 発火側 (ViewportNode) と受信側 (ViewportDirector) が互いを参照せずに済む。
    /// static シングルトンの代わりに、Inspector でアセット参照を渡す。
    /// </summary>
    [CreateAssetMenu(menuName = "EscapeGame/Channels/Viewport Request", fileName = "Ch_ViewportRequest")]
    public sealed class ViewportRequestChannel : ScriptableObject
    {
        public event Action<ViewpointSO> Requested;

        public void Raise(ViewpointSO target) => Requested?.Invoke(target);

        private void OnDisable() => Requested = null;
    }

    
}
