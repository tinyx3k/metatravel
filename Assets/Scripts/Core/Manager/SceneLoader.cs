using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.Utilities;
using Mapbox.Utils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Util;

namespace Core.Manager
{
    public class SceneLoader : MonoBehaviour
    {
        [Serializable]
        private class SceneLocation
        {
            public string name;
            [Geocode] public string location;
            public GameObject prefab;
        }

        private class Identifier : MonoBehaviour, IPointerClickHandler
        {
            public event Action OnClick;

            public void OnPointerClick(PointerEventData eventData)
            {
                OnClick?.Invoke();
            }
        }

        [SerializeField] private AbstractMap map;
        [SerializeField] private Canvas drawer;
        [SerializeField] private Animator transitionAnimator;
        [SerializeField] private float duration;
        [SerializeField] private Camera raycastCamera;
        [SerializeField] private List<SceneLocation> sceneCollection;

        private static readonly int StartTrigger = Animator.StringToHash("Start");
        private List<RectTransform> _spawnedObjects;
        private Vector2d[] _locations;

        private void Start()
        {
            _spawnedObjects = new List<RectTransform>();
            _locations = new Vector2d[sceneCollection.Count];
            var i = 0;
            foreach (var sceneLocation in sceneCollection)
            {
                _locations[i++] = Conversions.StringToLatLon(sceneLocation.location);
                var marker = Instantiate(sceneLocation.prefab, drawer.transform);
                var rect = marker.GetComponent<RectTransform>();
                marker.AddComponent<Identifier>().OnClick += () => StartCoroutine(LoadScene(sceneLocation.name));
                UnityUtils.BottomLeftAnchor(ref rect);
                rect.pivot = new Vector2(0.5f, 0.5f);
                marker.gameObject.layer = UnityUtils.LayerMaskToLayer(raycastCamera.cullingMask);
                _spawnedObjects.Add(rect);
            }

            map.OnInitialized += UpdateLocation;
            map.OnUpdated += UpdateLocation;
        }

        private void OnDestroy()
        {
            map.OnUpdated -= UpdateLocation;
        }

        private void UpdateLocation()
        {
            var count = _spawnedObjects.Count;
            for (var i = 0; i < count; i++)
            {
                var spawnedObject = _spawnedObjects[i];
                spawnedObject.anchoredPosition = LocationToScreenPoint(_locations[i]);
            }
        }

        private Vector2 LocationToScreenPoint(Vector2d location)
        {
            var worldLocation = map.GeoToWorldPosition(location);
            var relativePos = raycastCamera.WorldToViewportPoint(worldLocation);
            var viewportPoint = new Vector2(
                relativePos.x * Screen.width,
                relativePos.y * Screen.height);
            return viewportPoint;
        }

        private IEnumerator LoadScene(string sceneName)
        {
            if (transitionAnimator != null)
                transitionAnimator.SetTrigger(StartTrigger);
            yield return new WaitForSeconds(duration);
            SceneManager.LoadScene(sceneName);
        }
    }
}