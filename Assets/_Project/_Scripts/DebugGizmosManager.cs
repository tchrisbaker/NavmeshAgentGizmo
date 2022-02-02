using UnityEngine;

namespace tchrisbaker {
    [AddComponentMenu("_tchrisbaker/Debugging/Debug Gizmos Manager")]
    public class DebugGizmosManager : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        private RaycastHit LookForObjectsToDebug(out bool success)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
          
            RaycastHit hit;
            success = Physics.SphereCast(ray, 1f, out hit, Mathf.Infinity, layerMask);
            //Debug.Log($"DebugGizmosManager Successfully hit {hit.collider.name} ", hit.collider);
            return hit;
        }
        
        private void ToggleMovementGizmo(MovementGizmo movementGizmo)
        {
            if (movementGizmo)
            {
                //Debug.Log("Toggle MovementGizmo", movementGizmo);
                movementGizmo.DebugToggle();
            }
        }
        private Camera _camera;
        private void Start() => _camera = Camera.main;
        private void Update()
        {
          
            var toggleMovement = Input.GetKeyUp(KeyCode.L);
            if (toggleMovement)
            {
                var hit = LookForObjectsToDebug(out var success);
                if (success)
                {
                    var movementGizmo = hit.collider.GetComponentInChildren<MovementGizmo>();
                
                    
                    ToggleMovementGizmo(movementGizmo);
                    
                }
            }
        }
    }
}
