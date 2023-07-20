using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(MeshSequenceLoader))]
public class IMeshSequenceEditorManager : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        MeshSequenceLoader meshSequenceLoader = (MeshSequenceLoader)target;

        if(meshSequenceLoader.IsUsingMaterialSequence)
        {
            SetPropertyField("ExampleMaterialName", meshSequenceLoader.IsUsingMaterialSequence);
            SetPropertyField("MaterialSequence", meshSequenceLoader.IsUsingMaterialSequence);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetPropertyField(string name, bool set)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(name), set);
    }
}
