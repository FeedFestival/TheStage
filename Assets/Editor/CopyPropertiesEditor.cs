using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CopyProperties))]
public class CopyPropertiesEditor : Editor
{
    Vector2 scroll;
    private bool isCopied;
    private string text;
    public override void OnInspectorGUI()
    {
        CopyProperties myTarget = (CopyProperties)target;

        // Show default inspector property editor
        DrawDefaultInspector();

        if (GUILayout.Button("Copy"))
        {
            isCopied = true;
            text = myTarget.GetProperties();
        }
        if (isCopied)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            text = EditorGUILayout.TextArea(text, GUILayout.Height(200));
            EditorGUILayout.EndScrollView();
        }
    }
}
