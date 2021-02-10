using System.Collections;
using System.Collections.Generic;
using Tools.VisualEffects;
using UnityEngine;

public class RagdollController : MonoBehaviour {
    public CharacterBodyParts CharacterBodyParts;
    public float DestroySleepTime;
    public float DissolveTime;

    public void CopyTransforms(CharacterBodyParts bodyParts) {
        CharacterBodyParts.Head.position = bodyParts.Head.position;
        CharacterBodyParts.Neck.position = bodyParts.Neck.position;
        CharacterBodyParts.Chest.position = bodyParts.Chest.position;
        CharacterBodyParts.Pelvis.position = bodyParts.Pelvis.position;
        CharacterBodyParts.ArmUpNear.position = bodyParts.ArmUpNear.position;
        CharacterBodyParts.ArmDownNear.position = bodyParts.ArmDownNear.position;
        CharacterBodyParts.ArmUpRear.position = bodyParts.ArmUpRear.position;
        CharacterBodyParts.ArmDownRear.position = bodyParts.ArmDownRear.position;
        CharacterBodyParts.LegUpNear.position = bodyParts.LegUpNear.position;
        CharacterBodyParts.LegDownNear.position = bodyParts.LegDownNear.position;
        CharacterBodyParts.LegUpRear.position = bodyParts.LegUpRear.position;
        CharacterBodyParts.LegDownRear.position = bodyParts.LegDownRear.position;

        CharacterBodyParts.Head.rotation = bodyParts.Head.rotation;
        CharacterBodyParts.Neck.rotation = bodyParts.Neck.rotation;
        CharacterBodyParts.Chest.rotation = bodyParts.Chest.rotation;
        CharacterBodyParts.Pelvis.rotation = bodyParts.Pelvis.rotation;
        CharacterBodyParts.ArmUpNear.rotation = bodyParts.ArmUpNear.rotation;
        CharacterBodyParts.ArmDownNear.rotation = bodyParts.ArmDownNear.rotation;
        CharacterBodyParts.ArmUpRear.rotation = bodyParts.ArmUpRear.rotation;
        CharacterBodyParts.ArmDownRear.rotation = bodyParts.ArmDownRear.rotation;
        CharacterBodyParts.LegUpNear.rotation = bodyParts.LegUpNear.rotation;
        CharacterBodyParts.LegDownNear.rotation = bodyParts.LegDownNear.rotation;
        CharacterBodyParts.LegUpRear.rotation = bodyParts.LegUpRear.rotation;
        CharacterBodyParts.LegDownRear.rotation = bodyParts.LegDownRear.rotation;
    }

    public void Play() {
        StartCoroutine(DestroyTask());
    }

    private IEnumerator DestroyTask() {
        var spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        var chestRB = CharacterBodyParts.Chest.GetComponent<Rigidbody2D>();
        var sleepTimer = 0f;
        while(true) {
            var velMagnitude = chestRB.velocity.sqrMagnitude;
            if (velMagnitude < 0.1f)
                sleepTimer += Time.deltaTime;
            else
                sleepTimer = 0f;
            if (sleepTimer >= DestroySleepTime) {
                break;
            }
            yield return null;
        }
        var dissolveTimer = 0f;
        while (dissolveTimer < DissolveTime) {
            var fraction = 1 - (dissolveTimer / DissolveTime);
            var alpha = fraction;
            foreach(var sprite in spriteRenderers) {
                var color = sprite.color;
                sprite.color = new Color(color.r, color.g, color.b, alpha);
            }
            dissolveTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}