using System;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;

namespace Core.Map
{
    public class GameObjectCollectionSpawner : MonoBehaviour
    {
        [SerializeField] private AbstractMap map;
        [SerializeField] private List<GameObjectMarker> markerCollection;

        private List<GameObject> _spawnedObjects = new List<GameObject>();
        private Vector2d[] _locations;
        private void Start()
        {
            var i = 0;
            _locations = new Vector2d[markerCollection.Count];
            foreach (var marker in markerCollection)
            {
                _locations[i++] = Conversions.StringToLatLon(marker.locationString);
                var instance = Instantiate(marker.prefab);
                instance.transform.rotation = Quaternion.Euler(marker.rotation);
                instance.transform.localScale = marker.scale;
                instance.transform.localPosition = map.GeoToWorldPosition(_locations[i-1]);
                _spawnedObjects.Add(instance);
            }
        }

        private void Update()
        {
            var count = _spawnedObjects.Count;
            for (var i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                var location = _locations[i];
                spawnedObject.transform.localPosition = map.GeoToWorldPosition(location);
            }
        }
    }
}