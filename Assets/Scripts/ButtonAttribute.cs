using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ButtonAttribute : PropertyAttribute {

    public readonly string MethodName;
    public readonly string ButtonLabel;

    public ButtonAttribute(string method = null, string label = null) {
        MethodName = method;
        ButtonLabel = label;
    }
}
