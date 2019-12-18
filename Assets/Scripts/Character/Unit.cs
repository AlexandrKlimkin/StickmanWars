using Character.MuscleSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour {
    public MuscleController Controller { get; private set; }
    public List<Bone> Bones => GetComponentsInChildren<Bone>().ToList();
    public Bone Head => Bones.FirstOrDefault(_ => _.Type == MuscleType.Head);
    //public Bone

    private void Awake()
    {
        Controller = GetComponent<MuscleController>();
        //GetComponentsInChildren(Bones);
        //Head = Bones.FirstOrDefault(_ => _.Type == MuscleType.Head);
    }
}
