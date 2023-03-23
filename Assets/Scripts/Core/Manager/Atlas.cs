using System.Collections.Generic;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Data;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

namespace Core.Manager
{
    public class Atlas : MonoBehaviour
    {
        [SerializeField] private List<GameObject> affectedObjects;
        [SerializeField] private InputActionReference enableAction;
        [SerializeField] private InputActionReference disableAction;
        public AbstractMap abstractMap;
        public LayerMask parentLayer;
        private int _layer;

        private void Awake()
        {
            _layer = UnityUtils.LayerMaskToLayer(parentLayer);
            if (abstractMap == null) return;
            abstractMap.gameObject.layer = _layer;
            abstractMap.OnTileFinished += SetTileLayer;
        }

        private void OnEnable()
        {
            enableAction.action.performed += _ => PerformAtlas(true);
            disableAction.action.performed += _ => PerformAtlas(false);
            enableAction.action.Enable();
            disableAction.action.Enable();
        }

        private void OnDisable()
        {
            enableAction.action.Disable();
            disableAction.action.Disable();
        }

        private void OnDestroy()
        {
            enableAction.action.Dispose();
            disableAction.action.Dispose();
            if (abstractMap == null) return;
            abstractMap.OnTileFinished -= SetTileLayer;
        }

        private void PerformAtlas(bool isEnable)
        {
            if (isEnable)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            foreach (var gObject in affectedObjects)
            {
                gObject.SetActive(isEnable);
            }
        }

        private void SetTileLayer(UnityTile tile)
        {
            tile.gameObject.layer = _layer;
        }
    }
}
