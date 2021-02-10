using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class RagdollGenerationWindow : EditorWindow {

    public GameObject CharacterPrefab;
    public float MassMult = 1f;
    //public Material DefaultMaterial;

    public RagdollController Ragdoll;

    [MenuItem("Window/Custom/RagdollGeneration")]
    private static void Init() {
        RagdollGenerationWindow window = (RagdollGenerationWindow)GetWindow(typeof(RagdollGenerationWindow));
        window.Show();
    }

    private void OnGUI() {
        CharacterPrefab = (GameObject)EditorGUILayout.ObjectField(CharacterPrefab, typeof(GameObject), true);
        //DefaultMaterial = (Material)EditorGUILayout.ObjectField(DefaultMaterial, typeof(Material), true);
        if (GUILayout.Button("Generate")) {
            Ragdoll = RagdollCreator.GenerateRagdoll(CharacterPrefab);
        }
        Ragdoll = (RagdollController)EditorGUILayout.ObjectField(Ragdoll, typeof(RagdollController), true);
        MassMult = EditorGUILayout.FloatField(MassMult);
        if (GUILayout.Button("Auto mass")) {
            RagdollCreator.UseAutoMass(Ragdoll);
        }
        if (GUILayout.Button("Multiply mass")) {
            RagdollCreator.MultiplyMasses(Ragdoll, MassMult);
        }
    }

    private void AddJoint(Transform ragdoll, string part1Name, string part2Name, string ccdName) {
        var ccd = RecursiveFindChild(CharacterPrefab.transform, ccdName);
        var charPart1 = RecursiveFindChild(CharacterPrefab.transform, part1Name);
        var charPart2 = RecursiveFindChild(CharacterPrefab.transform, part2Name);

        var part1 = ragdoll.Find(part1Name);
        var part2 = ragdoll.Find(part2Name);
        if (ccd == null || charPart1 == null || charPart2 == null || part1 == null || part2 == null)
            return;
        var jointLocalPos = charPart1.InverseTransformPoint(ccd.position);

        var rb1 = part1.GetComponent<Rigidbody2D>();
        var rb2 = part2.GetComponent<Rigidbody2D>();
        var hingeJoint = part1.gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.connectedBody = rb2;
        hingeJoint.anchor = jointLocalPos;
    }

    public Transform RecursiveFindChild(Transform parent, string childName) {
        foreach (Transform child in parent) {
            if (child.name == childName) {
                return child;
            } else {
                Transform found = RecursiveFindChild(child, childName);
                if (found != null) {
                    return found;
                }
            }
        }
        return null;
    }

    private void SetParent(Transform ragdoll, string parentPartName, string childPartName) {
        var parentPart = ragdoll.Find(parentPartName);
        var childPart = ragdoll.Find(childPartName);
        childPart.SetParent(parentPart);
        DestroyImmediate(childPart.GetComponent<Rigidbody2D>()); // Destroy not need child rb
    }

    private T CopyComponent<T>(T original, GameObject destination) where T : Component {
        Type type = original.GetType();
        var dst = destination.GetComponent(type) as T;
        if (!dst) dst = destination.AddComponent(type) as T;
        var fields = type.GetFields();
        foreach (var field in fields) {
            if (field.IsStatic) continue;
            field.SetValue(dst, field.GetValue(original));
        }
        var props = type.GetProperties();
        foreach (var prop in props) {
            if (!prop.CanWrite || !prop.CanWrite || prop.Name == "name") continue;
            prop.SetValue(dst, prop.GetValue(original, null), null);
        }
        return dst as T;
    }
}
