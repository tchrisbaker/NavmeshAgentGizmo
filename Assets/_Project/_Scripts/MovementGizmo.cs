using System;
using UnityEngine;

namespace tchrisbaker {
    [AddComponentMenu("_tchrisbaker/Debugging/Movement Gizmo")]
    public class MovementGizmo : MonoBehaviour
    {
        
        private IMove _move;
        private Color color;

        [SerializeField, Header("Debug Details"), Range(0f, 5f)]
        private float offset = 2f;

        enum DebugType
        {
            SPHERE,
            CUBE,
            WIRED_CUBE,
            WIRED_SPHERE

        }

        [SerializeField, Header("Debugging")] private DebugType debugTypeToShow;
        private bool _doDebug;
    
       
        private void OnDrawGizmos()
        {
            if (_doDebug && _move != null)
                UnitDestinationGizmos();
        }

        private void UnitDestinationGizmos()
        {
            var targetPosition = _move.GetCurrentDestination() + Vector3.up * offset;
            var transformPosition = transform.position + Vector3.up * offset;
            switch (debugTypeToShow)
            {
                case DebugType.SPHERE:
                    ShowDefaultSphere(transformPosition, targetPosition, false);
                    break;
                case DebugType.WIRED_SPHERE:
                    ShowDefaultSphere(transformPosition, targetPosition, true);
                    break;
                case DebugType.CUBE:
                    ShowCube(transformPosition, targetPosition, false);
                    break;
                case DebugType.WIRED_CUBE:
                    ShowCube(transformPosition, targetPosition, true);
                    break;
            }
        }

        private void ShowDefaultSphere(Vector3 transformPosition, Vector3 targetPosition, bool isWired)
        {
            SetColor();
            if (isWired) Gizmos.DrawWireSphere(transformPosition, .25f);
            else Gizmos.DrawSphere(transformPosition, .25f);
            if (isWired) Gizmos.DrawWireSphere(targetPosition, .25f);
            else Gizmos.DrawSphere(targetPosition, .25f);
            DrawTargetLine(transformPosition, targetPosition);
        }

        private void SetColor()
        {
            color = Color.gray;
            var move = _move;
            if (!move.IsAgentValid() || move.IsStopped()) color = Color.red;
            else if (move.HasDestination() && move.AtDestination()) color = Color.green;
            else if (move.HasDestination() && !move.AtDestination()) color = Color.magenta;
            else if (!move.HasDestination() && move.AtDestination()) color = Color.yellow;
            else if (!move.HasDestination() && !move.AtDestination()) color = Color.cyan;

            Gizmos.color = color;
        }

        private void DrawTargetLine(Vector3 transformPosition, Vector3 targetPosition)
        {
            SetColor();
            Gizmos.DrawLine(transformPosition, targetPosition);
        }

        private void ShowCube(Vector3 transformPosition, Vector3 targetPosition, bool isWired)
        {
            SetColor();
            if (isWired) Gizmos.DrawWireCube(transformPosition, Vector3.one * .25f);
            else Gizmos.DrawCube(transformPosition, Vector3.one * .25f);
            if (isWired) Gizmos.DrawWireCube(targetPosition, Vector3.one * .25f);
            else Gizmos.DrawCube(targetPosition, Vector3.one * .25f);
            DrawTargetLine(transformPosition, targetPosition);
        }

        public void DebugToggle() => _doDebug = !_doDebug;

        public void Init(IMove move) => _move = move;

    }
}
