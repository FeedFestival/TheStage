using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.utils;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActManager)), CanEditMultipleObjects]
public class ActManagerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        ActManager myTarget = (ActManager)target;

        var screenXp = percent.Find(_percent: 25, _of: Screen.width);
        
        EditorGUILayout.BeginHorizontal();
        Rect resetBut = new Rect(
            0, 0,
            screenXp,
            25
            );
        if (GUI.Button(resetBut, "Reset Start"))
        {
            myTarget.gameObject.GetComponent<ActController>().SetStartAtAct("");
        }

        Rect resetSBut = new Rect(
            screenXp + 15, 0,
            screenXp,
            25
            );
        if (GUI.Button(resetSBut, "Reset Speed"))
        {
            myTarget.gameObject.GetComponent<ActController>().SetStartAtAct("");
        }

        // GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();

        // Show default inspector property editor
        DrawDefaultInspector();
    }
}
