using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Assets.Code.ServerPart.Physics.Data
{
    [CreateAssetMenu(fileName = "PhysicsShapeStorage", menuName = "ScriptableObjects/PhysicsShapeStorage", order = 51)]
    public sealed class PhysicsShapesStorage : ScriptableObject, IPhysicsShapesProvider
    {
        [SerializeField] private List<StoredPhysicsShape> _data = new();
        private Dictionary<string, int> _cachedShapes;

        PhysicsShape[] IPhysicsShapesProvider.GetShapeForPrefab(string prefabName)
        {
            if (_cachedShapes == null)
                CacheShapes();

            if (_cachedShapes.TryGetValue(prefabName, out var index))
                return _data[index].Shapes;

            Debug.LogError("Shape not found: " + prefabName);
            return new PhysicsShape[0];
        }

        private void CacheShapes()
        {
            _cachedShapes = new();
            for (int i = 0; i < _data.Count; i++)
            {
                StoredPhysicsShape shape = _data[i];
                _cachedShapes.Add(shape.PrefabName, i);
            }
        }


#if UNITY_EDITOR
        [ContextMenu("RebuildData")]
        public void RebuildData()
        {
            _data.Clear();

            string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { "Assets/Resources/Prefabs/Stations" });

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                var colliders = prefab.GetComponentsInChildren<SphereCollider>(true);

                if (colliders.Length == 0)
                    continue;

                var shapes = new PhysicsShape[colliders.Length];

                for (int i = 0; i < colliders.Length; i++)
                {
                    var c = colliders[i];

                    var localPos = c.transform.parent.localPosition + c.center;
                    var locaCenter = new double2(
                        localPos.x,
                        localPos.z);

                    var maxScale = Mathf.Max(
                        c.transform.lossyScale.x,
                        c.transform.lossyScale.y,
                        c.transform.lossyScale.z);
                    shapes[i] = new PhysicsShape(locaCenter, c.radius * maxScale);
                }

                var prefabKey = path
                    .Replace("Assets/Resources/", "")
                    .Replace(".prefab", "");

                _data.Add(new StoredPhysicsShape
                {
                    PrefabName = prefabKey,
                    Shapes = shapes
                });
            }

            EditorUtility.SetDirty(this);
        }
#endif
    }

    [Serializable]
    public struct StoredPhysicsShape
    {
        public string PrefabName;
        public PhysicsShape[] Shapes;
    }
}
