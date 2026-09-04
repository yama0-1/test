using EscapeGame.Interaction;
using EscapeGame.Inventory;
using EscapeGame.Viewport;
using System.Linq;
using UnityEngine;

namespace EscapeGame.Core
{
    /// <summary>
    /// タップ／クリックでレイキャストし、当たった IInteractable に Interact を投げる。
    /// プレイヤーは具象型 (ViewportNode, PickupInteractable …) を一切知らない。
    /// 新しい相互作用対象を追加してもこのクラスは変更不要。
    /// </summary>
    public sealed class PlayerInteractionInput : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private CameraMover _cameraMover;
        [SerializeField] private ViewDirector _viewDirector;
        [SerializeField] private InventoryManager _inventory;
        [SerializeField] private float _maxDistance = 100f;

        private void Update()
        {
            if (_cameraMover.IsAnimating) return;

            if (!WasTapped()) return;

            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            var context = new InteractionContext(_viewDirector.CurrentView,_inventory.Model.SelectedItem);

            RaycastHit[] hits = Physics.RaycastAll(ray, _maxDistance);

            foreach (var hit in hits.OrderBy(h => h.distance))
            {
                var interactables = hit.collider.GetComponents<Interactable>();

                bool interactedObj = false;//一度に複数のinteractableオブジェに触るのを回避
                foreach (var interactable in interactables)
                {
                    if (interactable.TryInteract(context)) interactedObj = true;
                }
                if(interactedObj) break;
            }
            
        }

        private static bool WasTapped()
        {
            if (Input.GetMouseButtonDown(0)) return true;
            return Input.touchCount > 0 &&
                   Input.GetTouch(0).phase == TouchPhase.Began;
        }
    }
}
