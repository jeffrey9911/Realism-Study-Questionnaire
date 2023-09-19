using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterAnimationPlayer))]
public class CCPlayerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();

        CharacterAnimationPlayer characterAnimationPlayer = (CharacterAnimationPlayer)target;

        if (characterAnimationPlayer.isPlayingAudio)
        {
            SetPropertyField("PlayerAudio", characterAnimationPlayer.isPlayingAudio);
        }

        if (characterAnimationPlayer.isPlayingAudio)
        {
            float EditorAudioOffset = EditorGUILayout.Slider("Audio Player Offset (Sec)", characterAnimationPlayer.AudioPlayOffset, 0, 10f);
            if (EditorAudioOffset != characterAnimationPlayer.AudioPlayOffset)
            {
                float dOffset = EditorAudioOffset - characterAnimationPlayer.AudioPlayOffset;
                characterAnimationPlayer.OnAudioOffsetChanged(dOffset);
                characterAnimationPlayer.AudioPlayOffset = EditorAudioOffset;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    void SetPropertyField(string name, bool set)
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(name), set);
    }
}
