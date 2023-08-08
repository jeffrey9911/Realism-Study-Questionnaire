using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MeshSequenceLoader))]
public class IMeshSequenceLoaderEditorManager : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        MeshSequenceLoader meshSequenceLoader = (MeshSequenceLoader)target;

        if(meshSequenceLoader.IsUsingMaterialSequence)
        {
            SetPropertyField("ExampleMaterial", meshSequenceLoader.IsUsingMaterialSequence);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetPropertyField(string name, bool set)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(name), set);
    }
}


[CustomEditor(typeof(MeshSequencePlayer))]
public class IMeshSequencePlayerEditorManager : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        MeshSequencePlayer meshSequencePlayer = (MeshSequencePlayer)target;

        if (meshSequencePlayer.isPlayingAudio)
        {
            SetPropertyField("PlayerAudio", meshSequencePlayer.isPlayingAudio);
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetPropertyField(string name, bool set)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(name), set);
    }
}