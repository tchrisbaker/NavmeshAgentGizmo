using UnityEngine;

namespace tchrisbaker {
    public static class Extensions 
    {    
        public static Vector3 DirectionTo (this Vector3 source, Vector3 destination ) => Vector3.Normalize(destination-source);
    }
}
