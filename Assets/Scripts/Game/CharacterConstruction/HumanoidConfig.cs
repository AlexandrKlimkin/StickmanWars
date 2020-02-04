using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Character.MuscleSystem;
using UnityEngine;

namespace CharacterConstruction
{
    [CreateAssetMenu(fileName = "HumanoidConfig", menuName = "Configs/HumanoidConfig")]
    public class HumanoidConfig : ScriptableObject
    {
        [SerializeField]
        private List<BoneGenerationData> _GenerationData;
        private Dictionary<MuscleType, BoneGenerationData> BoneGenerationDataDict;

        private void OnEnable()
        {
            BoneGenerationDataDict = _GenerationData.ToDictionary(_ => _.MuscleType);
        }

        public BoneGenerationData GetGenerationData(MuscleType muscleType)
        {
            return BoneGenerationDataDict.ContainsKey(muscleType) ? BoneGenerationDataDict[muscleType] : null;
        }
    }
}