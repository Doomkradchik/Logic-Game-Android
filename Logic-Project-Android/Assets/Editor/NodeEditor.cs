using System.Collections.Generic;
using PathCreation;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Node))]
public class NodeEditor : Editor
{
    private SerializedProperty _nodesProperty;
    private SerializedProperty _isActiveProperty;

    private GameObject _wirePrefab;
    private List<Node> _cache;

    private void OnEnable()
    {
        _nodesProperty = serializedObject.FindProperty("_nodes");
        _isActiveProperty = serializedObject.FindProperty("_isActive");

        _wirePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Wire.prefab");

        RefreshCache();
    }

    public override void OnInspectorGUI()
    {
         serializedObject.Update();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(_isActiveProperty);
        DisplayArray(_nodesProperty);

        if (GUILayout.Button("Remove") && _nodesProperty.arraySize > 0)
        {
            _nodesProperty.arraySize--;

            if(_cache[_cache.Count - 1] != null)
            if (TryFindByLastName(_cache[_cache.Count - 1].name, out GameObject obj))
                DestroyImmediate(obj);
        }
        else
        if (GUILayout.Button("Add"))
        {
            _nodesProperty.arraySize++;
            var size = _nodesProperty.arraySize;
            _nodesProperty.GetArrayElementAtIndex(size - 1).objectReferenceValue = null;
        }
          
        if (EditorGUI.EndChangeCheck())
            OnGUIItemsChanged();

        RefreshCache();
        serializedObject.ApplyModifiedProperties();
    }

    private void DisplayArray(SerializedProperty array, string caption = null)
    {
        EditorGUILayout.LabelField((caption == null) ? array.name : caption);
        for (int i = 0; i < array.arraySize; i++)
        {
            EditorGUILayout.PropertyField(array.GetArrayElementAtIndex(i));
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void OnGUIItemsChanged()
    {
        if (_cache.Count() != _nodesProperty.arraySize)
            return;

        for (int i = 0; i < _nodesProperty.arraySize; i++)
        {
            var saved = _cache[i];
            SerializedProperty property = _nodesProperty.GetArrayElementAtIndex(i);
            Node node = property.objectReferenceValue as Node;

            if (saved == node)
                continue;

            var found = TryFindByLastName(saved == null ? node.name : saved.name, out GameObject wire);

            if (node == null)
            {
                if (found)
                    DestroyImmediate(wire);
                continue;
            }

            if (found == false)
            {
                var instance = Instantiate(_wirePrefab);
                instance.transform.SetParent(((Node)target).transform);
                instance.transform.localPosition = Vector3.zero;
                wire = instance;
            }

            UpdateName(wire, node.name);
            UpdatePath(wire.GetComponent<PathCreator>(), node.transform.localPosition);
        }
    }

    private void RefreshCache()
    {
        _cache = new List<Node>();
        for (int i = 0; i < _nodesProperty.arraySize; i++)
        {
            SerializedProperty property = _nodesProperty.GetArrayElementAtIndex(i);
            _cache.Add(property.objectReferenceValue as Node);
        }
    }

    private void UpdateName(GameObject wire, string lastName)
    {
        var name = wire.name.Split(':')[0];
        wire.name = $"{name}:{lastName}";
    }

    private void UpdatePath(PathCreator pathCreator, Vector3 point)
    {
        var vertices = new Vector3[]
        {
             Vector3.zero,
            -((Node)target).transform.localPosition + point
        };
        BezierPath bezierPath = new BezierPath(vertices, false, PathSpace.xy);
        pathCreator.bezierPath = bezierPath;
    }

    private bool TryFindByLastName(string lastName, out GameObject result)
    {
        result = null;
        var wires = ((Node)target).GetComponentsInChildren<Transform>()
                  .Skip(1);

        foreach (var wire in wires)
        {
            var tag = wire.name.Split(':')[1];

            if (tag == lastName)
            {
                result = wire.gameObject;
                return true;
            }
        }

        return false;
    }
}
