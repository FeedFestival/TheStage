using UnityEngine;
using UnityEditor;
using Assets.Scripts.Utils;

public enum Speeds {
    NormalSpeed,
    FastSpeed,
    LightSpeed
};

[CustomPropertyDrawer(typeof(ActGameObject))]
public class ActGameObjectPropertyDrawer : PropertyDrawer
{
    string[] _speeds = new [] { Speeds.NormalSpeed.ToString(), Speeds.FastSpeed.ToString(), Speeds.LightSpeed.ToString() };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // base.OnGUI(position, property, label);

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck();

        Rect foldPos = new Rect(position.x + 0, position.y + 0, position.width, 15);

        Rect startHLabelPos = new Rect(
            position.x + UsefullUtils.GetPercent(position.width, 0), position.y + 15,
            UsefullUtils.GetPercent(position.width, 25),
            25
            );
        Rect speedPos = new Rect(
            position.x + UsefullUtils.GetPercent(position.width, 30), position.y + 15,
            UsefullUtils.GetPercent(position.width, 25),
            25
            );
        Rect goPos = new Rect(
            position.x + UsefullUtils.GetPercent(position.width, 60), position.y + 15,
            UsefullUtils.GetPercent(position.width, 40),
            25
            );

        SerializedProperty p = property.FindPropertyRelative("_editor_foldout");
        if (p.boolValue = EditorGUI.Foldout(foldPos, p.boolValue, property.displayName))
        {
            GUI.Label(foldPos, property.displayName);
            
            if (GUI.Button(startHLabelPos, "Start Here"))
            {
                (property.serializedObject.targetObject as ActManager).GetComponent<ActController>()
                    .SetStartAtAct(property.propertyPath);
            }

            GameObject value = (GameObject)EditorGUI.ObjectField(
                goPos, "",
                (property.FindPropertyRelative("Go").objectReferenceValue as GameObject),
                typeof(GameObject), true
                );

            property.FindPropertyRelative("_choiceIndex").intValue 
                = EditorGUI.Popup(speedPos, property.FindPropertyRelative("_choiceIndex").intValue, _speeds);
            // var someClass = target as SomeClass;
            // Update the selected choice in the underlying object
            // someClass.choice = _choices[_choiceIndex];
            // Save the changes back to the object
            // EditorUtility.SetDirty(target);

            if (EditorGUI.EndChangeCheck())
            {
                property.FindPropertyRelative("Go").objectReferenceValue = value;
                property.serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
        }
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty p = property.FindPropertyRelative("_editor_foldout");
        if (p.boolValue)
        {
            return base.GetPropertyHeight(property, label) + 30;
        }
        else
        {
            return base.GetPropertyHeight(property, label) + 0;
        }
    }
}
