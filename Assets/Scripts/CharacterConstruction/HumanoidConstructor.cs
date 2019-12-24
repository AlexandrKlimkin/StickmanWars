using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using CharacterConstruction;
using UnityEngine;

namespace CharacterConstruction
{
    public class HumanoidConstructor : UnitCostructor
    {
        private enum AxisType { Up, Down, Middle }

        public override Unit ConstructUnit(string unitId)
        {
            var root = new GameObject(unitId).transform;
            var sizeData = GenerateSizeData(unitId);

            var chest = CreateBone(unitId, "Chest", sizeData.ChestSize, root, null, AxisType.Up, 1f, 0.5f);
            var neck = CreateBone(unitId, "Neck", sizeData.NeckSize, root, chest.UpAxis, AxisType.Down, 0.4f, 0.4f);
            var head = CreateScaledBone(unitId, "Head", sizeData.HeadScale, root, neck.UpAxis, AxisType.Down);

            var hipUp = CreateBone(unitId, "HipUp", sizeData.HipUpSize, root, chest.DownAxis, AxisType.Up, 0.8f, 1f);
            var hipDown = CreateBone(unitId, "HipDown", sizeData.HipDownSize, root, hipUp.DownAxis, AxisType.Up, sizeData.LegMiddleSize.x / 2, 0.4f);
            var hip = new GameObject("Hip");
            hip.transform.position = hipUp.transform.position;
            hip.transform.SetParent(root);
            hipUp.transform.SetParent(hip.transform);
            hipDown.transform.SetParent(hip.transform);

            var legUpNear = CreateBone(unitId, "LegUpNear", sizeData.LegUpSize, root, hipDown.DownAxis, AxisType.Up, sizeData.LegMiddleSize.x / 2, 0.4f);
            var legUpBack = CreateBone(unitId, "LegUpBack", sizeData.LegUpSize, root, hipDown.DownAxis, AxisType.Up, sizeData.LegMiddleSize.x / 2, 0.4f);

            var legMiddleNear = CreateBone(unitId, "LegMiddleNear", sizeData.LegMiddleSize, root, legUpNear.DownAxis, AxisType.Up, sizeData.LegDownSize.x / 2, 0.4f);
            var legMiddleBack = CreateBone(unitId, "LegMiddleBack", sizeData.LegMiddleSize, root, legUpBack.DownAxis, AxisType.Up, sizeData.LegDownSize.x / 2, 0.4f);

            var legDownNear = CreateBone(unitId, "LegDownNear", sizeData.LegDownSize, root, legMiddleNear.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var legDownBack = CreateBone(unitId, "LegDownBack", sizeData.LegDownSize, root, legMiddleBack.DownAxis, AxisType.Up, 0.4f, 0.4f);

            var bootNear = CreateScaledBone(unitId, "BootNear", sizeData.BootScale, root, legDownNear.DownAxis, AxisType.Up);
            var bootBack = CreateScaledBone(unitId, "BootBack", sizeData.BootScale, root, legDownBack.DownAxis, AxisType.Up);

            var armUpNear = CreateBone(unitId, "ArmUpNear", sizeData.ArmUpSize, root, chest.MidleAxis, AxisType.Up, 0.4f, 0.4f);
            var armUpBack = CreateBone(unitId, "ArmUpBack", sizeData.ArmUpSize, root, chest.MidleAxis, AxisType.Up, 0.4f, 0.4f);

            var armDownNear = CreateBone(unitId, "ArmDownNear", sizeData.ArmDownSize, root, armUpNear.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var armDownBack = CreateBone(unitId, "ArmDownBack", sizeData.ArmDownSize, root, armUpBack.DownAxis, AxisType.Up, 0.4f, 0.4f);

            var fistNear = CreateScaledBone(unitId, "FistNear", sizeData.FistScale, root, armDownNear.DownAxis, AxisType.Up);
            var fistBack = CreateScaledBone(unitId, "FistBack", sizeData.FistScale, root, armDownBack.DownAxis, AxisType.Up);

            //Joints
            var hipJoint = hipUp.gameObject.AddComponent<HingeJoint2D>();
            hipJoint.connectedBody = chest.Rigidbody;
            hipJoint.anchor = hipUp.DownAxis.localPosition;

            var neckJoint = neck.gameObject.AddComponent<HingeJoint2D>();
            neckJoint.connectedBody = chest.Rigidbody;
            neckJoint.anchor = neck.DownAxis.localPosition;

            var headJoint = head.gameObject.AddComponent<HingeJoint2D>();
            headJoint.connectedBody = neck.Rigidbody;
            headJoint.anchor = head.DownAxis.localPosition;

            var unit = root.gameObject.AddComponent<Unit>();
            unit.gameObject.AddComponent<Removecollision>();
            return unit;
        }


        private HumanoidSizeData GenerateSizeData(string unitId)
        {
            var config = Resources.Load<HumanoidConfig>($"Characters/{unitId}/HumanoidConfig");

            var chestData = config.GetGenerationData(MuscleType.Chest);
            var chestSize = IntervalsToSize(chestData.WidthRandom, chestData.HeightRandom);

            var hipUpData = config.GetGenerationData(MuscleType.HipUp);
            var hipUpSize = IntervalsToSize(hipUpData.WidthRandom, hipUpData.HeightRandom);
            hipUpSize = new Vector2(chestSize.x, hipUpSize.y);

            var hipDownData = config.GetGenerationData(MuscleType.HipDown);
            var hipDownSize = IntervalsToSize(hipDownData.WidthRandom, hipDownData.HeightRandom);
            hipDownSize = new Vector2(Mathf.Clamp(hipDownSize.x, hipUpSize.x / 3 * 2, hipUpSize.x / 10 * 9), hipDownSize.y);

            var neckData = config.GetGenerationData(MuscleType.Neck);
            var neckSize = IntervalsToSize(neckData.WidthRandom, neckData.HeightRandom);

            var headData = config.GetGenerationData(MuscleType.Head);
            var headScale = Random.Range(headData.ScaleXRandom.x, headData.ScaleXRandom.y);

            var legUpData = config.GetGenerationData(MuscleType.LegUp);
            var legUpSize = IntervalsToSize(legUpData.WidthRandom, legUpData.HeightRandom);
            legUpSize = new Vector2(Mathf.Clamp(legUpSize.x, 0, hipDownSize.x / 4 * 3), legUpSize.y);

            var legMiddleData = config.GetGenerationData(MuscleType.LegMiddle);
            var legMiddleSize = IntervalsToSize(legMiddleData.WidthRandom, legMiddleData.HeightRandom);
            legMiddleSize = new Vector2(Mathf.Clamp(legMiddleSize.x, 0, legUpSize.x / 4 * 3), legMiddleSize.y);

            var legDownData = config.GetGenerationData(MuscleType.LegDown);
            var legDownSize = IntervalsToSize(legDownData.WidthRandom, legDownData.HeightRandom);
            legDownSize = new Vector2(Mathf.Clamp(legDownSize.x, 0, legMiddleSize.x / 4 * 3), legDownSize.y);

            var bootData = config.GetGenerationData(MuscleType.Boot);
            var bootScale = Random.Range(bootData.ScaleXRandom.x, bootData.ScaleXRandom.y);

            var armUpData = config.GetGenerationData(MuscleType.ArmUp);
            var armUpSize = IntervalsToSize(armUpData.WidthRandom, armUpData.HeightRandom);

            var armDownData = config.GetGenerationData(MuscleType.ArmDown);
            var armDownSize = IntervalsToSize(armDownData.WidthRandom, armDownData.HeightRandom);

            var fistData = config.GetGenerationData(MuscleType.Fist);
            var fistScale = Random.Range(fistData.ScaleXRandom.x, fistData.ScaleXRandom.y);

            return new HumanoidSizeData()
            {
                ChestSize = chestSize,
                HipUpSize = hipUpSize,
                HipDownSize = hipDownSize,
                NeckSize = neckSize,
                HeadScale = headScale,
                LegUpSize = legUpSize,
                LegMiddleSize = legMiddleSize,
                LegDownSize = legDownSize,
                ArmUpSize = armUpSize,
                ArmDownSize = armDownSize,
                BootScale = bootScale,
                FistScale = fistScale
            };
        }

        private Vector2 IntervalsToSize(Vector2 randX, Vector2 randY)
        {
            return new Vector2(Random.Range(randX.x, randX.y), Random.Range(randY.x, randY.y));
        }

        private Bone CreateScaledBone(string unitId, string name, float scale, Transform root, Transform connectedAxis, AxisType axisType)
        {
            var prefab = Resources.Load<Bone>($"Characters/{unitId}/Bones/{name}");
            var bone = Object.Instantiate(prefab, root);

            bone.transform.localScale = new Vector3(scale,scale,scale);
            if (connectedAxis != null) {
                Transform axis = null;
                switch (axisType) {
                    case AxisType.Down:
                        axis = bone.DownAxis;
                        break;
                    case AxisType.Middle:
                        axis = bone.MidleAxis;
                        break;
                    case AxisType.Up:
                        axis = bone.UpAxis;
                        break;
                }
                bone.transform.position = connectedAxis.position - axis.localPosition * bone.transform.localScale.x;
            }

            bone.Rigidbody = bone.gameObject.AddComponent<Rigidbody2D>();

            return bone;
        }

        private Bone CreateBone(string unitId, string name, Vector2 size, Transform root, Transform connectedAxis, AxisType axisType, float downAxisOffset, float upAxisOffset)
        {
            var prefab = Resources.Load<Bone>($"Characters/{unitId}/Bones/{name}");
            var bone = Object.Instantiate(prefab, root);

            foreach (var spriteRenderer in bone.SpriteRenderers)
            {
                spriteRenderer.drawMode = SpriteDrawMode.Sliced;
                spriteRenderer.size = size;
            }
            bone.CapsuleCollider.size = size;
            if (bone.DownAxis != null)
                bone.DownAxis.localPosition = new Vector3(0, -size.y / 2 + downAxisOffset, 0);
            if(bone.UpAxis != null)
                bone.UpAxis.localPosition = new Vector3(0, size.y / 2 - upAxisOffset, 0);
            if(bone.MidleAxis != null)
                bone.MidleAxis.localPosition = new Vector3(0, size.y / 4);

            if (connectedAxis != null)
            {
                Transform axis = null;
                switch (axisType)
                {
                    case AxisType.Down:
                        axis = bone.DownAxis;
                        break;
                    case AxisType.Middle:
                        axis = bone.MidleAxis;
                        break;
                    case AxisType.Up:
                        axis = bone.UpAxis;
                        break;
                }

                bone.transform.position = connectedAxis.position - axis.localPosition;
            }

            bone.Rigidbody = bone.gameObject.AddComponent<Rigidbody2D>();

            return bone;
        }


    }
}
