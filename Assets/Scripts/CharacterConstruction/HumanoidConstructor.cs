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

            var chest = CreateBone(unitId, "Chest", sizeData.ChestSize, root, null, AxisType.Up, 0.4f, 0.5f);
            var neck = CreateBone(unitId, "Neck", sizeData.NeckSize, root, chest.UpAxis, AxisType.Down, 0.4f, 0.4f);
            var head = CreateScaledBone(unitId, "Head", sizeData.HeadScale, root, neck.UpAxis, AxisType.Down);
            var hip = CreateBone(unitId, "Hip", sizeData.HipSize, root, chest.DownAxis, AxisType.Up, 0.8f, 0.5f);
            var legUpNear = CreateBone(unitId, "LegUpNear", sizeData.LegUpSize, root, hip.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var legUpBack = CreateBone(unitId, "LegUpBack", sizeData.LegUpSize, root, hip.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var legDownNear = CreateBone(unitId, "LegDownNear", sizeData.LegDownSize, root, legUpNear.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var legDownBack = CreateBone(unitId, "LegDownBack", sizeData.LegDownSize, root, legUpBack.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var bootNear = CreateScaledBone(unitId, "BootNear", sizeData.BootScale, root, legDownNear.DownAxis, AxisType.Up);
            var bootBack = CreateScaledBone(unitId, "BootBack", sizeData.BootScale, root, legDownBack.DownAxis, AxisType.Up);
            var armUpNear = CreateBone(unitId, "ArmUpNear", sizeData.ArmUpSize, root, chest.MidleAxis, AxisType.Up, 0.4f, 0.4f);
            var armUpBack = CreateBone(unitId, "ArmUpBack", sizeData.ArmUpSize, root, chest.MidleAxis, AxisType.Up, 0.4f, 0.4f);
            var armDownNear = CreateBone(unitId, "ArmDownNear", sizeData.ArmDownSize, root, armUpNear.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var armDownBack = CreateBone(unitId, "ArmDownBack", sizeData.ArmDownSize, root, armUpBack.DownAxis, AxisType.Up, 0.4f, 0.4f);
            var fistNear = CreateScaledBone(unitId, "FistNear", sizeData.FistScale, root, armDownNear.DownAxis, AxisType.Up);
            var fistBack = CreateScaledBone(unitId, "FistBack", sizeData.FistScale, root, armDownBack.DownAxis, AxisType.Up);
            return root.gameObject.AddComponent<Unit>();
        }


        private HumanoidSizeData GenerateSizeData(string unitId)
        {

            var config = Resources.Load<HumanoidConfig>($"Characters/{unitId}/HumanoidConfig");

            var chestData = config.GetGenerationData(MuscleType.Chest);
            var chestSize = IntervalsToSize(chestData.WidthRandom, chestData.HeightRandom);

            var hipData = config.GetGenerationData(MuscleType.Hip);
            var hipSize = IntervalsToSize(hipData.WidthRandom, hipData.HeightRandom);
            hipSize = new Vector2(Mathf.Clamp(hipSize.x, 0, chestSize.x - 0.2f), hipSize.y);

            var neckData = config.GetGenerationData(MuscleType.Neck);
            var neckSize = IntervalsToSize(neckData.WidthRandom, neckData.HeightRandom);

            var headData = config.GetGenerationData(MuscleType.Head);
            var headScale = Random.Range(headData.ScaleXRandom.x, headData.ScaleXRandom.y);

            var legUpData = config.GetGenerationData(MuscleType.LegUp);
            var legUpSize = IntervalsToSize(legUpData.WidthRandom, legUpData.HeightRandom);     

            var legDownData = config.GetGenerationData(MuscleType.LegDown);
            var legDownSize = IntervalsToSize(legDownData.WidthRandom, legDownData.HeightRandom);

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
                HipSize = hipSize,
                NeckSize = neckSize,
                HeadScale = headScale,
                LegUpSize = legUpSize,
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
            return bone;
        }
    }
}
