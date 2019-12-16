using System.Collections;
using System.Collections.Generic;
using Character.MuscleSystem;
using CharacterConstruction;
using UnityEngine;

namespace CharacterConstruction
{
    public class HumanoidConstructor : UnitCostructor
    {

        enum AxisType { Up, Down, Middle }

        public override Unit ConstructUnit(string unitId)
        {
            var root = new GameObject(unitId).transform;

            var chest = CreateBone(unitId, MuscleType.Chest.ToString(), root, null, AxisType.Up);
            var neck = CreateBone(unitId, MuscleType.Neck.ToString(), root, chest.UpAxis, AxisType.Down);
            var head = CreateBone(unitId, MuscleType.Head.ToString(), root, neck.UpAxis, AxisType.Down);
            var hip = CreateBone(unitId, MuscleType.Hip.ToString(), root, chest.DownAxis, AxisType.Up);
            var legUpNear = CreateBone(unitId, "LegUpNear", root, hip.DownAxis, AxisType.Up);
            var legUpBack = CreateBone(unitId, "LegUpBack", root, hip.DownAxis, AxisType.Up);
            var legDownNear = CreateBone(unitId, "LegDownNear", root, legUpNear.DownAxis, AxisType.Up);
            var legDownBack = CreateBone(unitId, "LegDownBack", root, legUpBack.DownAxis, AxisType.Up);
            var armUpNear = CreateBone(unitId, "ArmUpNear", root, chest.MidleAxis, AxisType.Up);
            var armUpBack = CreateBone(unitId, "ArmUpBack", root, chest.MidleAxis, AxisType.Up);
            var armDownNear = CreateBone(unitId, "ArmDownNear", root, armUpNear.DownAxis, AxisType.Up);
            var armDownBack = CreateBone(unitId, "ArmDownBack", root, armUpBack.DownAxis, AxisType.Up);

            return root.gameObject.AddComponent<Unit>();
        }

        private Bone CreateBone(string unitId, string name, Transform root, Transform connectedAxis, AxisType axisType)
        {
            var prefab = Resources.Load<Bone>($"Characters/{unitId}/{name}");
            var bone = Object.Instantiate(prefab, root);
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
