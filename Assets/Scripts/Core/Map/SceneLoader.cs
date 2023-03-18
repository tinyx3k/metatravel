using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Map
{
    public class SceneLoader: MonoBehaviour
    {
        [Serializable]
        private class SceneLocation
        {
            [Geocode] public string location;
            public GameObject prefab;
            public Vector3 rotation;
            public Vector3 scale = Vector3.one;
            public SceneAsset scene;
        }

        [SerializeField] private AbstractMap map;
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private float duration;
        [SerializeField] private List<SceneLocation> sceneCollection;
 
        private static readonly int StartTrigger = Animator.StringToHash( "Start");
        private List<GameObject> _spawnedObjects;
        private Vector2d[] _locations;

        private void Start()
        {
            _spawnedObjects = new List<GameObject>();
            _locations = new Vector2d[sceneCollection.Count];
            var i = 0;
            foreach (var sceneLocation in sceneCollection)
            {
                _locations[i++] = Conversions.StringToLatLon(sceneLocation.location);
                var marker = GenerateLocationMarker(sceneLocation);
                marker.transform.localPosition = map.GeoToWorldPosition(_locations[i-1]);
                _spawnedObjects.Add(marker);
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
                if (Camera.main == null) continue;
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var raycastHit, 100f)) continue;
                if (raycastHit.transform == null) continue;
                print(sceneCollection[i].scene.name);
            }
        }

        private GameObject GenerateLocationMarker(SceneLocation marker)
        {
            var instance = Instantiate(marker.prefab);
            instance.transform.rotation = Quaternion.Euler(marker.rotation);
            instance.transform.localScale = marker.scale;
            return instance;
        }
        
        private IEnumerator LoadScene(SceneAsset scene)
        {
            if (transitionAnimator != null)
                transitionAnimator.SetTrigger(StartTrigger);
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene(scene.name);
        }
    }
}