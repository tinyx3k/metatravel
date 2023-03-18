using System;
using Mapbox.Unity.Utilities;
using UnityEngine;

namespace Core.Map
{
    [Serializable]
    public struct GameObjectMarker
    {
        public Vector3 rotation;
        public Vector3 scale;
        public GameObject prefab;
        [Geocode]
        public string locationString;
    }
}