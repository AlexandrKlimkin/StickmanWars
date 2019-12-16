using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var button = attribute as ButtonAttribute;
        var owner = (object)property.serializedObject.targetObject;

        var buttonLabel = button.ButtonLabel ?? label.text;
        var methodName = button.MethodName ?? "On" + property.name.TrimStart('_');
        var click = GUI.Button(position, buttonLabel);
        if (click) {
            var type = owner.GetType();
            var method = type.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            if (method == null) {
                Debug.LogError("Method not found");
                return;
            }
            method.Invoke(owner, new object[0]);
        }
    }
}