using UnityEngine;
using UnityEngine.AI;

namespace tchrisbaker {
    public class RandomMovement : MonoBehaviour
    {
        private IMove _move;
        [SerializeField] float range = 10f;
        
        private void Start() => SetDestination();

        private void SetDestination() => _move.SetDestination(_move.PickLocationInRange(range));

        private void OnUnitReachedDestination() => SetDestination();

        private void OnEnable()
        {
            _move = new NavMeshAgentMove(GetComponent<NavMeshAgent>());
            GetComponent<MovementGizmo>().Init(_move);
            _move.OnHasReachedDestinationEvent += OnUnitReachedDestination;
        }

        void OnDestroy() => _move.OnHasReachedDestinationEvent -= OnUnitReachedDestination;
    }
}
