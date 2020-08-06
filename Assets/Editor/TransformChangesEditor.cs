using Assets.Scripts.Utils;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(TransformChanges))]
public class TransformChangesEditor : Editor
{
    private static GUIStyle ToggleButtonStyleNormal = null;
    private static GUIStyle ToggleButtonStyleToggled = null;

    private TransformChanges myTarget;

    private float topY;
    private float topX;

    private float height = 20;
    private float insideFoldWidth;
    private float insideFoldSecondLevelWidth;
    private float topXSecondLevel;

    public override void OnInspectorGUI()
    {
        if (ToggleButtonStyleNormal == null)
        {
            ToggleButtonStyleNormal = "Button";
            ToggleButtonStyleToggled = new GUIStyle(ToggleButtonStyleNormal);
            ToggleButtonStyleToggled.normal.background = ToggleButtonStyleToggled.active.background;
        }

        myTarget = (TransformChanges)target;

        var minHeight = 30;
        if (myTarget.OriginalFoldout) {
            minHeight = 150;
        }
        if (myTarget.ToSecondFoldout) {
            minHeight = 200;
        }
        var componentHeight = ((topY + 30) < minHeight) ? minHeight : (topY + 30);
        EditorGUILayout.BeginHorizontal(GUILayout.Width(Screen.width), GUILayout.Height(componentHeight));

        EditorGUILayout.Space(5);

        topY = 5;
        topX = 20;
        Rect buttonPos = new Rect(topX - 10, topY, Screen.width, 100);

        GUILayout.BeginArea(buttonPos);
        EditorGUILayout.BeginHorizontal();

        myTarget.TransformAnchorPos = GUILayout.Toggle(myTarget.TransformAnchorPos, "Anchor Pos", "Button",
            GUILayout.Width(UsefullUtils.GetPercent(Screen.width, 20)), GUILayout.Height(height));
        myTarget.TransformDelta = GUILayout.Toggle(myTarget.TransformDelta, "Delta", "Button",
            GUILayout.Width(UsefullUtils.GetPercent(Screen.width, 20)), GUILayout.Height(height));
        myTarget.TransformRotation = GUILayout.Toggle(myTarget.TransformRotation, "Rotation", "Button",
            GUILayout.Width(UsefullUtils.GetPercent(Screen.width, 20)), GUILayout.Height(height));

        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
        //

        insideFoldWidth = Screen.width - 40;

        if (myTarget.TransformAnchorPos || myTarget.TransformDelta || myTarget.TransformRotation)
        {
            OriginalFold();
            ToSecondFold();
            topY += 25;
            Rect foldPos = new Rect(topX - 5, topY, Screen.width, 15);
            if (myTarget.MoreThenOne = EditorGUI.Foldout(foldPos, myTarget.MoreThenOne, "More"))
            {
                insideFoldSecondLevelWidth = Screen.width - 60;
                topXSecondLevel = topX + 20;
                ToThirdFold();
                ToForthFold();
            }
        }
        else
        {
            topY += 25;
            Rect textPos = new Rect(topX - 5, topY, Screen.width, 30);
            GUILayout.BeginArea(textPos);
            EditorGUILayout.LabelField("Nothing to change so you should remove it!");
            GUILayout.EndArea();
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.EndVertical();

        EditorUtility.SetDirty(myTarget);
        EditorSceneManager.MarkSceneDirty(myTarget.gameObject.scene);
    }

    private void OriginalFold()
    {
        topY += 25;
        Rect foldPos = new Rect(topX - 5, topY, Screen.width, 15);
        if (myTarget.OriginalFoldout = EditorGUI.Foldout(foldPos, myTarget.OriginalFoldout, "Original"))
        {
            topY += height;
            Rect buttonLock = new Rect(topX, topY, UsefullUtils.GetPercent(insideFoldWidth, 10), height);
            Rect buttonSet = new Rect(topX + UsefullUtils.GetPercent(insideFoldWidth, 10), topY, UsefullUtils.GetPercent(insideFoldWidth, 40), height);
            Rect buttonView = new Rect(topX + UsefullUtils.GetPercent(insideFoldWidth, 50), topY, UsefullUtils.GetPercent(insideFoldWidth, 50), height);
            if (GUI.Button(buttonLock, myTarget.OriginalLocked ? "Unlock" : "Lock"))
            {
                myTarget.OriginalLocked = !myTarget.OriginalLocked;
            }
            EditorGUI.BeginDisabledGroup(myTarget.OriginalLocked);
            if (GUI.Button(buttonSet, "Set Current" + (myTarget.OriginalLocked ? " (L)" : "")))
            {
                if (myTarget.OriginalLocked)
                {
                    return;
                }
                myTarget.SetCurrent();
            }
            EditorGUI.EndDisabledGroup();

            if (GUI.Button(buttonView, "View Now"))
            {
                myTarget.ViewNow();
            }

            EditorGUI.BeginDisabledGroup(myTarget.OriginalLocked);

            if (myTarget.TransformAnchorPos)
            {
                topY += height;
                Rect origPos = new Rect(topX, topY, Screen.width, height);
                GUILayout.BeginArea(origPos);
                myTarget.OriginalAnchorPos = EditorGUILayout.Vector3Field("Anchor Pos", myTarget.OriginalAnchorPos,
                            GUILayout.Height(height), GUILayout.Width(insideFoldWidth));
                GUILayout.EndArea();
            }

            if (myTarget.TransformDelta)
            {
                topY += height;
                Rect origDelta = new Rect(topX, topY, Screen.width, height);
                GUILayout.BeginArea(origDelta);
                myTarget.OriginalScaleDelta = EditorGUILayout.Vector2Field("Original Delta", myTarget.OriginalScaleDelta,
                            GUILayout.Height(height), GUILayout.Width(insideFoldWidth));
                GUILayout.EndArea();
            }

            if (myTarget.TransformRotation)
            {
                topY += height;
                Rect origRot = new Rect(topX, topY, Screen.width, height);
                GUILayout.BeginArea(origRot);
                myTarget.OriginalRotation = EditorGUILayout.Vector3Field("Original Rotation", myTarget.OriginalRotation,
                            GUILayout.Height(height), GUILayout.Width(insideFoldWidth));
                GUILayout.EndArea();
            }

            EditorGUI.EndDisabledGroup();
        }
    }

    private void ToSecondFold()
    {
        topY += 25;
        Rect foldPos = new Rect(topX - 5, topY, Screen.width, 15);
        if (myTarget.ToSecondFoldout = EditorGUI.Foldout(foldPos, myTarget.ToSecondFoldout, "To Second"))
        {
            topY += height;
            Rect buttonLock = new Rect(topX, topY, UsefullUtils.GetPercent(insideFoldWidth, 10), height);
            Rect buttonSet = new Rect(topX + UsefullUtils.GetPercent(insideFoldWidth, 10), topY, UsefullUtils.GetPercent(insideFoldWidth, 40), height);
            Rect buttonView = new Rect(topX + UsefullUtils.GetPercent(insideFoldWidth, 50), topY, UsefullUtils.GetPercent(insideFoldWidth, 50), height);
            if (GUI.Button(buttonLock, myTarget.SecondLocked ? "Unlock" : "Lock"))
            {
                myTarget.SecondLocked = !myTarget.SecondLocked;
            }
            EditorGUI.BeginDisabledGroup(myTarget.SecondLocked);
            if (GUI.Button(buttonSet, "Set Current" + (myTarget.SecondLocked ? " (L)" : "")))
            {
                if (myTarget.SecondLocked)
                {
                    return;
                }
                myTarget.SetCurrentSecondary();
            }
            EditorGUI.EndDisabledGroup();

            if (GUI.Button(buttonView, "View Now"))
            {
                myTarget.ViewNowSecondary();
            }

            EditorGUI.BeginDisabledGroup(myTarget.SecondLocked);

            if (myTarget.TransformAnchorPos)
            {
                topY += height;
                Rect origPos = new Rect(topX, topY, Screen.width, height);
                GUILayout.BeginArea(origPos);
                myTarget.SecondAnchorPos = EditorGUILayout.Vector3Field("Anchor Pos", myTarget.SecondAnchorPos,
                            GUILayout.Height(height), GUILayout.Width(insideFoldWidth));
                GUILayout.EndArea();
            }

            if (myTarget.TransformDelta)
            {
                topY += height;
                Rect origDelta = new Rect(topX, topY, Screen.width, height);
                GUILayout.BeginArea(origDelta);
                myTarget.SecondScaleDelta = EditorGUILayout.Vector2Field("Delta", myTarget.SecondScaleDelta,
                            GUILayout.Height(height), GUILayout.Width(insideFoldWidth));
                GUILayout.EndArea();
            }

            if (myTarget.TransformRotation)
            {
                topY += height;
                Rect origRot = new Rect(topX, topY, Screen.width, height);
                GUILayout.BeginArea(origRot);
                myTarget.SecondRotation = EditorGUILayout.Vector3Field("Rotation", myTarget.SecondRotation,
                            GUILayout.Height(height), GUILayout.Width(insideFoldWidth));
                GUILayout.EndArea();
            }

            EditorGUI.EndDisabledGroup();
        }
    }

    private void ToThirdFold()
    {
        topY += 25;
        Rect secondToggled = new Rect(topX, topY, insideFoldWidth, 100);

        GUILayout.BeginArea(secondToggled);
        myTarget.UseThird = GUILayout.Toggle(myTarget.UseThird, "Third", "Button",
            GUILayout.Width(UsefullUtils.GetPercent(Screen.width, 10)), GUILayout.Height(height));
        GUILayout.EndArea();

        if (myTarget.UseThird)
        {
            topY += 25;
            Rect foldPos = new Rect(topXSecondLevel - 5, topY, Screen.width, 15);
            if (myTarget.ToThirdFoldout = EditorGUI.Foldout(foldPos, myTarget.ToThirdFoldout, "To Third"))
            {
                topY += height;
                Rect buttonLock = new Rect(topXSecondLevel, topY, UsefullUtils.GetPercent(insideFoldSecondLevelWidth, 10), height);
                Rect buttonSet = new Rect(topXSecondLevel + UsefullUtils.GetPercent(insideFoldSecondLevelWidth, 10), topY, UsefullUtils.GetPercent(insideFoldSecondLevelWidth, 40), height);
                Rect buttonView = new Rect(topXSecondLevel + UsefullUtils.GetPercent(insideFoldSecondLevelWidth, 50), topY, UsefullUtils.GetPercent(insideFoldSecondLevelWidth, 50), height);
                if (GUI.Button(buttonLock, myTarget.ThirdLocked ? "Unlock" : "Lock"))
                {
                    myTarget.ThirdLocked = !myTarget.ThirdLocked;
                }
                EditorGUI.BeginDisabledGroup(myTarget.ThirdLocked);
                if (GUI.Button(buttonSet, "Set Current" + (myTarget.ThirdLocked ? " (L)" : "")))
                {
                    if (myTarget.ThirdLocked)
                    {
                        return;
                    }
                    myTarget.SetCurrentThird();
                }
                EditorGUI.EndDisabledGroup();

                if (GUI.Button(buttonView, "View Now"))
                {
                    myTarget.ViewNowThird();
                }

                EditorGUI.BeginDisabledGroup(myTarget.ThirdLocked);

                if (myTarget.TransformAnchorPos)
                {
                    topY += height;
                    Rect origPos = new Rect(topXSecondLevel, topY, Screen.width, height);
                    GUILayout.BeginArea(origPos);
                    myTarget.ThirdAnchorPos = EditorGUILayout.Vector3Field("Anchor Pos", myTarget.ThirdAnchorPos,
                                GUILayout.Height(height), GUILayout.Width(insideFoldSecondLevelWidth));
                    GUILayout.EndArea();
                }

                if (myTarget.TransformDelta)
                {
                    topY += height;
                    Rect origDelta = new Rect(topXSecondLevel, topY, Screen.width, height);
                    GUILayout.BeginArea(origDelta);
                    myTarget.ThirdScaleDelta = EditorGUILayout.Vector2Field("Delta", myTarget.ThirdScaleDelta,
                                GUILayout.Height(height), GUILayout.Width(insideFoldSecondLevelWidth));
                    GUILayout.EndArea();
                }

                if (myTarget.TransformRotation)
                {
                    topY += height;
                    Rect origRot = new Rect(topXSecondLevel, topY, Screen.width, height);
                    GUILayout.BeginArea(origRot);
                    myTarget.ThirdRotation = EditorGUILayout.Vector3Field("Rotation", myTarget.ThirdRotation,
                                GUILayout.Height(height), GUILayout.Width(insideFoldSecondLevelWidth));
                    GUILayout.EndArea();
                }

                EditorGUI.EndDisabledGroup();
            }
        }
    }

    private void ToForthFold()
    {
        topY += 25;
        Rect secondToggled = new Rect(topX, topY, insideFoldWidth, 100);

        GUILayout.BeginArea(secondToggled);
        myTarget.UseForth = GUILayout.Toggle(myTarget.UseForth, "Forth", "Button",
            GUILayout.Width(UsefullUtils.GetPercent(Screen.width, 10)), GUILayout.Height(height));
        GUILayout.EndArea();

    }
}
