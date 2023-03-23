using System;
using UnityEngine;

namespace Util
{
    public static class UnityUtils
    {
        #region LayerUtils
        public static int LayerMaskToLayer(LayerMask layerMask) {
            return (int) Mathf.Log(layerMask.value, 2);
        }

        public static void IgnoreLayerFromCamera(ref Camera cam, int layer)
        {
            cam.cullingMask &=  ~(1 << layer);
        }

        public static void RecognizeLayerToCamera(ref Camera cam, int layer)
        {
            cam.cullingMask |= (1 << layer);
        }
        #endregion

        #region MeshUtils
        public static Vector3 GetTopVertexWorldPos(MeshFilter meshFilter, Transform transform)
        {
            var verts = meshFilter.sharedMesh.vertices;
            var topVertex = Vector3.zero;
            foreach (var vt in verts)
            {
                var vert = transform.TransformPoint(vt);
                if (vert.y <= topVertex.y) continue;
                topVertex = vert;
            }

            return topVertex;
        }
        #endregion

        #region TransformUtils
        public static Vector3 GetPositionAroundX(Vector3 origin, Vector3 target, float angle)
        {
            var distance = (target - origin).magnitude;
            var sinDis = Math.Sin(angle) * distance;
            var cosDis = Math.Cos(angle) * distance;
            return new Vector3(origin.x, (float)sinDis, (float)cosDis);
        }
        #endregion

        #region UI
        //------------Top-------------------
        public static void TopLeftAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(0, 1);
            uiTransform.anchorMax = new Vector2(0, 1);
        }
        public static void TopMiddleAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(0.5f, 1);
            uiTransform.anchorMax = new Vector2(0.5f, 1);
        }
        public static void TopRightAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(1, 1);
            uiTransform.anchorMax = new Vector2(1, 1);
        }
        //------------Middle-------------------
        public static void MiddleLeftAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(0, 0.5f);
            uiTransform.anchorMax = new Vector2(0, 0.5f);
        }
        public static void MiddleAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(0.5f, 0.5f);
            uiTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
        public static void MiddleRightAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(1, 0.5f);
            uiTransform.anchorMax = new Vector2(1, 0.5f);
        }
        //------------Bottom-------------------
        public static void BottomLeftAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(0, 0);
            uiTransform.anchorMax = new Vector2(0, 0);
        }
        public static void BottomMiddleAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(0.5f, 0);
            uiTransform.anchorMax = new Vector2(0.5f, 0);
        }
        public static void BottomRightAnchor(ref RectTransform uiTransform)
        {
            uiTransform.anchorMin = new Vector2(1, 0);
            uiTransform.anchorMax = new Vector2(1, 0);
        }
        #endregion
    }
}