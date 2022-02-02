using System;
using UnityEngine;

namespace tchrisbaker {
    public interface IMove
    {
        public event Action OnHasReachedDestinationEvent;
        public event Action OnHasSetNewDestinationEvent;
        public event Action OnHasClearedDestinationEvent;
        public bool AtDestination();
    
        public bool IsStopped();
     
        public Vector3 PickLocationInRange(float range, float nearestPointSearchRange = 5f);
        public void SetDestination(Vector3 destination, float nearestPointSearchRange = 5f);
        Vector3 GetCurrentDestination();
        
        bool HasDestination();
        bool IsAgentValid();
    }
}
