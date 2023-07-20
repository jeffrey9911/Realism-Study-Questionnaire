using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(QSComponent))]
public class QSCompEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        QSComponent qsComponent = (QSComponent)target;

        switch (qsComponent.ResponseType)
        {
            case ResponseMode.Slider:
                SetPropertyField("Slider", true);
                SetPropertyField("LeftText", true);
                SetPropertyField("RightText", true);
                SetPropertyField("HandleText", true);
                break;

            case ResponseMode.ColorSlider:
                SetPropertyField("Slider", true);
                SetPropertyField("HandleText", true);
                break;

            case ResponseMode.Toggle:
                SetPropertyField("toggle", true);
                SetPropertyField("toggleText", true);
                break;

            case ResponseMode.Text:
                SetPropertyField("InputField", true);
                break;

            default:
                break;
        }

        serializedObject.ApplyModifiedProperties();

    }

    void SetPropertyField(string name, bool set)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(name), set);
    }
}


