using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RagdollCreator {
    public static RagdollController GenerateRagdoll(GameObject characterPrefab) {
        if (characterPrefab == null) {
            Debug.LogError("Character prefab is null");
            return null;
        }
        if (!characterPrefab.GetComponent<CharacterUnit>()) {
            Debug.LogError("Object is not a character");
            return null;
        }
        var ragdollObject = new GameObject($"{characterPrefab.name}_ragdoll");
        var ragdoll = ragdollObject.AddComponent<RagdollController>();
        ragdollObject.AddComponent<Removecollision>();
        var allsprites = characterPrefab.GetComponentsInChildren<SpriteRenderer>();
        var newSprites = new List<SpriteRenderer>();
        foreach (var sprite in allsprites) {
            var spriteObj = sprite.gameObject;
            var newSpriteObj = new GameObject(spriteObj.name);
            var localPos = characterPrefab.transform.InverseTransformPoint(spriteObj.transform.position);
            var lossyScale = spriteObj.transform.lossyScale;
            newSpriteObj.transform.SetParent(ragdollObject.transform);
            newSpriteObj.transform.localPosition = localPos;
            newSpriteObj.transform.rotation = spriteObj.transform.rotation;
            newSpriteObj.transform.localScale = lossyScale;
            newSprites.Add(CopyComponent(sprite, newSpriteObj));
            //sprite.sharedMaterial = DefaultMaterial;
        }
        var colliders = new List<Collider2D>();
        var layer = LayerMask.NameToLayer(Layers.Names.Corpse);
        foreach (var sprite in newSprites) {
            var rb = sprite.gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 4f;
            var collider = sprite.gameObject.AddComponent<CapsuleCollider2D>();
            collider.gameObject.layer = layer;
            colliders.Add(collider);
            var simpledmgbl = sprite.gameObject.AddComponent<SimpleDamageable>();
        }

        foreach (var col in colliders) {
            foreach (var col1 in colliders) {
                Physics2D.IgnoreCollision(col, col1);
            }
        }
        AddJoint(characterPrefab, ragdollObject.transform, "Pelvis", "Chest", "ChestJoint", true, 300f, 380f);
        AddJoint(characterPrefab, ragdollObject.transform, "Chest", "Neck", "NeckJoint", true, 340f, 380f);
        AddJoint(characterPrefab, ragdollObject.transform, "Neck", "Head", "HeadJoint", true, 330f, 370f);

        AddJoint(characterPrefab, ragdollObject.transform, "Chest", "ArmUpNear", "ArmUpNearJoint");
        AddJoint(characterPrefab, ragdollObject.transform, "Chest", "ArmUpRear", "ArmUpRearJoint");
        AddJoint(characterPrefab, ragdollObject.transform, "ArmUpNear", "ArmDownNear", "ArmDownNearJoint", true, 350f, 470f);
        AddJoint(characterPrefab, ragdollObject.transform, "ArmUpRear", "ArmDownRear", "ArmDownRearJoint", true, 350f, 470f);

        AddJoint(characterPrefab, ragdollObject.transform, "Pelvis", "LegUpNear", "LegUpNearJoint", true, 280f, 460f);
        AddJoint(characterPrefab, ragdollObject.transform, "Pelvis", "LegUpRear", "LegUpRearJoint", true, 280f, 460f);
        AddJoint(characterPrefab, ragdollObject.transform, "LegUpNear", "LegDownNear", "LegDownNearJoint", true, 240f, 370f);
        AddJoint(characterPrefab, ragdollObject.transform, "LegUpRear", "LegDownRear", "LegDownRearJoint", true, 240f, 370f);

        SetParent(ragdollObject.transform, "ArmDownNear", "HandNear");
        SetParent(ragdollObject.transform, "ArmDownRear", "HandRear");
        SetParent(ragdollObject.transform, "LegDownNear", "FootNear");
        SetParent(ragdollObject.transform, "LegDownRear", "FootRear");

        ragdoll.CharacterBodyParts = new CharacterBodyParts {
            Head = RecursiveFindChild(ragdollObject.transform, "Head"),
            Neck = RecursiveFindChild(ragdollObject.transform, "Neck"),
            Chest = RecursiveFindChild(ragdollObject.transform, "Chest"),
            Pelvis = RecursiveFindChild(ragdollObject.transform, "Pelvis"),
            ArmUpNear = RecursiveFindChild(ragdollObject.transform, "ArmUpNear"),
            ArmDownNear = RecursiveFindChild(ragdollObject.transform, "ArmDownNear"),
            ArmUpRear = RecursiveFindChild(ragdollObject.transform, "ArmUpRear"),
            ArmDownRear = RecursiveFindChild(ragdollObject.transform, "ArmDownRear"),
            LegUpNear = RecursiveFindChild(ragdollObject.transform, "LegUpNear"),
            LegDownNear = RecursiveFindChild(ragdollObject.transform, "LegDownNear"),
            LegUpRear = RecursiveFindChild(ragdollObject.transform, "LegUpRear"),
            LegDownRear = RecursiveFindChild(ragdollObject.transform, "LegDownRear")
        };
        return ragdoll;
    }

    public static void UseAutoMass(RagdollController ragdoll) {
        var rbs = ragdoll.GetComponentsInChildren<Rigidbody2D>();
        foreach (var rb in rbs) {
            rb.useAutoMass = true;
        }
        var massSumm = 0f;
        foreach (var rb in rbs) {
            massSumm += rb.mass;
        }
        Debug.LogError($"Final mass = {massSumm}");
    }

    public static void MultiplyMasses(RagdollController ragdoll, float multiplier) {
        if (ragdoll == null)
            return;
        var rbs = ragdoll.GetComponentsInChildren<Rigidbody2D>();
        var massSumm = 0f;
        foreach(var rb in rbs) {
            rb.useAutoMass = false;
            rb.mass = rb.mass * multiplier;
            massSumm += rb.mass;
        }
        Debug.LogError($"Final mass = {massSumm}");
    }

    private static void AddJoint(GameObject characterPrefab, Transform ragdoll, string part1Name, string part2Name, string ccdName, bool useLimits = false, float minLimit = 0f, float maxLimit = 0f) {
        var ccd = RecursiveFindChild(characterPrefab.transform, ccdName);
        var charPart1 = RecursiveFindChild(characterPrefab.transform, part1Name);
        var charPart2 = RecursiveFindChild(characterPrefab.transform, part2Name);

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
        hingeJoint.useLimits = useLimits;
        if(useLimits)
            hingeJoint.limits = new JointAngleLimits2D {max = maxLimit, min = minLimit};
    }

    private static Transform RecursiveFindChild(Transform parent, string childName) {
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

    private static void SetParent(Transform ragdoll, string parentPartName, string childPartName) {
        var parentPart = ragdoll.Find(parentPartName);
        var childPart = ragdoll.Find(childPartName);
        childPart.SetParent(parentPart);
        GameObject.DestroyImmediate(childPart.GetComponent<Rigidbody2D>()); // Destroy not need components
        GameObject.DestroyImmediate(childPart.GetComponent<Collider2D>());
        GameObject.DestroyImmediate(childPart.GetComponent<SimpleDamageable>());
    }

    private static T CopyComponent<T>(T original, GameObject destination) where T : Component {
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
