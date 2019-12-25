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
            var legUp1 = new GameObject("LegUpNear");
            legUp1.transform.position = legUpNear.transform.position;
            legUp1.transform.SetParent(root);
            legUpNear.transform.SetParent(legUp1.transform);
            legMiddleNear.transform.SetParent(legUp1.transform);
            var legUp2 = new GameObject("LegUpBack");
            legUp2.transform.position = legUpBack.transform.position;
            legUp2.transform.SetParent(root);
            legUpBack.transform.SetParent(legUp2.transform);
            legMiddleBack.transform.SetParent(legUp2.transform);

            var legDownNear = CreateBone(unitId, "LegDownNear", sizeData.LegDownSize, root, legMiddleNear.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var legDownBack = CreateBone(unitId, "LegDownBack", sizeData.LegDownSize, root, legMiddleBack.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var bootNear = CreateScaledBone(unitId, "BootNear", sizeData.BootScale, root, legDownNear.DownAxis, AxisType.Up);
            var bootBack = CreateScaledBone(unitId, "BootBack", sizeData.BootScale, root, legDownBack.DownAxis, AxisType.Up);
            var legDown1 = new GameObject("LegDownNear");
            legDown1.transform.position = legDownNear.transform.position;
            legDown1.transform.SetParent(root);
            legDownNear.transform.SetParent(legDown1.transform);
            bootNear.transform.SetParent(legDown1.transform);
            var legDown2 = new GameObject("LegDownBack");
            legDown2.transform.position = legDownBack.transform.position;
            legDown2.transform.SetParent(root);
            legDownBack.transform.SetParent(legDown2.transform);
            bootBack.transform.SetParent(legDown2.transform);

            var armUpNear = CreateBone(unitId, "ArmUpNear", sizeData.ArmUpSize, root, chest.MidleAxis, AxisType.Up, 0.4f, 0.4f);
            var armUpBack = CreateBone(unitId, "ArmUpBack", sizeData.ArmUpSize, root, chest.MidleAxis, AxisType.Up, 0.4f, 0.4f);

            var armDownNear = CreateBone(unitId, "ArmDownNear", sizeData.ArmDownSize, root, armUpNear.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var armDownBack = CreateBone(unitId, "ArmDownBack", sizeData.ArmDownSize, root, armUpBack.DownAxis, AxisType.Up, 0.4f, 0.4f);

            var fistNear = CreateScaledBone(unitId, "FistNear", sizeData.FistScale, root, armDownNear.DownAxis, AxisType.Up);
            var fistBack = CreateScaledBone(unitId, "FistBack", sizeData.FistScale, root, armDownBack.DownAxis, AxisType.Up);

            //Rigidbodies
            chest.Rigidbody = chest.gameObject.AddComponent<Rigidbody2D>();
            neck.Rigidbody = neck.gameObject.AddComponent<Rigidbody2D>();
            head.Rigidbody = head.gameObject.AddComponent<Rigidbody2D>();
            var hipRb = hip.AddComponent<Rigidbody2D>();
            armUpNear.Rigidbody = armUpNear.gameObject.AddComponent<Rigidbody2D>();
            armUpBack.Rigidbody = armUpBack.gameObject.AddComponent<Rigidbody2D>();
            armDownNear.Rigidbody = armDownNear.gameObject.AddComponent<Rigidbody2D>();
            armDownBack.Rigidbody = armDownBack.gameObject.AddComponent<Rigidbody2D>();
            var legUpNearRb = legUp1.AddComponent<Rigidbody2D>();
            var legUpBackRb = legUp2.AddComponent<Rigidbody2D>();
            var legDownNearRb = legDown1.AddComponent<Rigidbody2D>();
            var legDownBackRb = legDown2.AddComponent<Rigidbody2D>();

            //Joints
            var hipJoint = hip.AddComponent<HingeJoint2D>();
            hipJoint.connectedBody = chest.Rigidbody;
            hipJoint.anchor = hipUp.UpAxis.localPosition;
            hipJoint.useLimits = true;
            hipJoint.limits = new JointAngleLimits2D { min = -120f, max = 25f };

            var neckJoint = neck.gameObject.AddComponent<HingeJoint2D>();
            neckJoint.connectedBody = chest.Rigidbody;
            neckJoint.anchor = neck.DownAxis.localPosition;
            neckJoint.useLimits = true;
            neckJoint.limits = new JointAngleLimits2D { min = -30f, max = 30f };

            var headJoint = head.gameObject.AddComponent<HingeJoint2D>();
            headJoint.connectedBody = neck.Rigidbody;
            headJoint.anchor = head.DownAxis.localPosition;
            headJoint.useLimits = true;
            headJoint.limits = new JointAngleLimits2D{ min = -30f, max = 30f};

            var armUpNearJoint = armUpNear.gameObject.AddComponent<HingeJoint2D>();
            armUpNearJoint.connectedBody = chest.Rigidbody;
            armUpNearJoint.anchor = armUpNear.UpAxis.localPosition;
            var armUpBackJoint = armUpBack.gameObject.AddComponent<HingeJoint2D>();
            armUpBackJoint.connectedBody = chest.Rigidbody;
            armUpBackJoint.anchor = armUpBack.UpAxis.localPosition;

            var armDownNearJoint = armDownNear.gameObject.AddComponent<HingeJoint2D>();
            armDownNearJoint.connectedBody = armUpNear.Rigidbody;
            armDownNearJoint.anchor = armDownNear.UpAxis.localPosition;
            armDownNearJoint.useLimits = true;
            armDownNearJoint.limits = new JointAngleLimits2D { min = -170f, max = 5f };
            var armDownBackJoint = armDownBack.gameObject.AddComponent<HingeJoint2D>();
            armDownBackJoint.connectedBody = armUpBack.Rigidbody;
            armDownBackJoint.anchor = armDownBack.UpAxis.localPosition;
            armDownBackJoint.useLimits = true;
            armDownBackJoint.limits = new JointAngleLimits2D { min = -170f, max = 5f };

            var fistNearJoint = fistNear.gameObject.AddComponent<HingeJoint2D>();
            fistNearJoint.connectedBody = armDownNear.Rigidbody;
            fistNearJoint.anchor = fistNear.UpAxis.localPosition;
            fistNearJoint.useLimits = true;
            fistNearJoint.limits = new JointAngleLimits2D { min = -20f, max = 20f };
            var fistBackJoint = fistBack.gameObject.AddComponent<HingeJoint2D>();
            fistBackJoint.connectedBody = armDownBack.Rigidbody;
            fistBackJoint.anchor = fistBack.UpAxis.localPosition;
            fistBackJoint.useLimits = true;
            fistBackJoint.limits = new JointAngleLimits2D { min = -20f, max = 20f };

            var legUpNearJoint = legUp1.AddComponent<HingeJoint2D>();
            legUpNearJoint.connectedBody = hipRb;
            legUpNearJoint.anchor = legUpNear.UpAxis.localPosition;
            legUpNearJoint.useLimits = true;
            legUpNearJoint.limits = new JointAngleLimits2D { min = -130f, max = 90f };
            var legUpBackJoint = legUp2.AddComponent<HingeJoint2D>();
            legUpBackJoint.connectedBody = hipRb;
            legUpBackJoint.anchor = legUpBack.UpAxis.localPosition;
            legUpBackJoint.useLimits = true;
            legUpBackJoint.limits = new JointAngleLimits2D { min = -130f, max = 90f };

            var legDownNearJoint = legDown1.AddComponent<HingeJoint2D>();
            legDownNearJoint.connectedBody = legUpNearRb;
            legDownNearJoint.anchor = legDownNear.UpAxis.localPosition;
            legDownNearJoint.useLimits = true;
            legDownNearJoint.limits = new JointAngleLimits2D { min = -5f, max = 150f };
            var legDownBackJoint = legDown2.AddComponent<HingeJoint2D>();
            legDownBackJoint.connectedBody = legUpBackRb;
            legDownBackJoint.anchor = legDownBack.UpAxis.localPosition;
            legDownBackJoint.useLimits = true;
            legDownBackJoint.limits = new JointAngleLimits2D { min = -5f, max = 150f };

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

            //bone.Rigidbody = bone.gameObject.AddComponent<Rigidbody2D>();

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

            //bone.Rigidbody = bone.gameObject.AddComponent<Rigidbody2D>();

            return bone;
        }


    }
}
