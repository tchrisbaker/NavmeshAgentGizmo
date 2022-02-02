using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace tchrisbaker {
    public class NavMeshAgentMove : IMove
    {
        private NavMeshAgent _agent;

        private Transform _transform;
        private bool _logging;
        private int _walkableAreaMask = 1 << NavMesh.GetAreaFromName("Walkable");
        private bool _destinationSet;
        private bool _reachedDestination;
        private int _checkReachedDestinationDelayInMS;
        private bool _hasApplicationQuit;
        public event Action OnHasReachedDestinationEvent;
        public event Action OnHasSetNewDestinationEvent;
        public event Action OnHasClearedDestinationEvent;

        public NavMeshAgentMove(NavMeshAgent agent, int checkReachedDestinationDelayInMS = 1000)
        {
            _agent = agent;
            _transform = agent.gameObject.transform;
            _checkReachedDestinationDelayInMS = checkReachedDestinationDelayInMS;
            RepeatCheckReachedDestination();
        }

        
        public Vector3 PickLocationInRange(float range, float nearestPointSearchRange = 5f)
        {
            Vector3 searchLocation = _transform.position;
            searchLocation += Random.Range(-range, range) * Vector3.forward;
            searchLocation += Random.Range(-range, range) * Vector3.right;

            NavMeshHit hitResult;
            if (NavMesh.SamplePosition(searchLocation, out hitResult, nearestPointSearchRange, _walkableAreaMask))
                return hitResult.position;

            return _transform.position;
        }


        public void SetDestination(Vector3 destination, float nearestPointSearchRange = 5f)
        {
            // find nearest spot on navmesh and move there
            NavMeshHit hitResult;
            if (NavMesh.SamplePosition(destination, out hitResult, nearestPointSearchRange, _walkableAreaMask))
            {
                HasSetNewDestination(hitResult.position);
            }
        }

        public Vector3 GetCurrentDestination() => _agent.destination;
        

        public bool AtDestination() => _reachedDestination;

        public bool HasDestination() => _destinationSet;
        public bool IsAgentValid() => _agent.enabled && _agent.isOnNavMesh;

        public bool IsStopped()
        {
            if (!IsAgentValid()) return true;
            return _agent.isStopped;
        }

        private void HasSetNewDestination(Vector3 destination)
        {
            Log($"{_agent.name} moving to {destination.ToString()}", _agent);
            if (_agent.isStopped) _agent.isStopped = false;
            _agent.SetDestination(destination);
            _destinationSet = true;
            _reachedDestination = false;
            OnHasSetNewDestinationEvent?.Invoke();
        }

        
        private void Log(string message, Object context)
        {
            if (_logging) Debug.Log(message, context );
        }

        async void RepeatCheckReachedDestination()
        {
            while (Application.isPlaying && !_hasApplicationQuit)
            {
                await Task.Delay(_checkReachedDestinationDelayInMS);
                CheckReachedDestination();
            }

        }
        public void CheckReachedDestination()
        {
            if (!Application.isPlaying || _hasApplicationQuit || !IsAgentValid())
                return;
            
            if (!_agent.pathPending && !_agent.isOnOffMeshLink && _destinationSet && (_agent.remainingDistance <= _agent.stoppingDistance))
            {
                HasReachedDestination();
            }
        }
        private void HasReachedDestination()
        {
            Log($"{_agent.name} Has Reached Destination", _agent);
            _destinationSet = false;
            _reachedDestination = true;
            OnHasReachedDestinationEvent?.Invoke();
        }
    }
}
