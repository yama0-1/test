using System.Collections;
using UnityEngine;

namespace EscapeGame.Viewport
{
    /// <summary>
    /// カメラを指定姿勢へ動かす。旧 CameraController.move / Rotatecamera を置き換える。
    ///  - public static isrotate を IsAnimating プロパティに変更 (グローバル状態の排除)。
    ///  - WaitForSeconds によるコマ送りを Time.deltaTime ベースに変更 (フレームレート非依存)。
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CameraMover : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private AnimationCurve _easing = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public bool IsAnimating { get; private set; }

        public void MoveCamera(ViewpointSO point)
        {
            StartCoroutine(MoveTo(point));
        }
        private IEnumerator MoveTo(ViewpointSO point)
        {
            IsAnimating = true;

            Vector3 fromPos = transform.position;
            Quaternion fromRot = transform.rotation;
            Vector3 toPos = point.CameraPosition;
            Quaternion toRot = Quaternion.Euler(point.CameraEulerAngles);

            float elapsed = 0f;
            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                float k = _easing.Evaluate(Mathf.Clamp01(elapsed / _duration));
                transform.SetPositionAndRotation(
                    Vector3.LerpUnclamped(fromPos, toPos, k),
                    Quaternion.SlerpUnclamped(fromRot, toRot, k));
                yield return null;
            }

            transform.SetPositionAndRotation(toPos, toRot);
            IsAnimating = false;
        }
    }
}
