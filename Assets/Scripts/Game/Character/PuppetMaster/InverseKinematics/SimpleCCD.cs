using UnityEngine;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SimpleCCD : MonoBehaviour
{
    public int Iterations = 5;

    [Range(0.01f, 1)] public float Damping = 1;

    public Transform Target;
    public Transform EndTransform;

    public Node[] Nodes = new Node[0];

    private Dictionary<Transform, Node> _NodeCache;
    private float[] _Angles;

    public bool DrawAnglesGizmos;

    [System.Serializable]
    public class Node
    {
        public Transform Transform;
        public float Min = 0;
        public float Max = 360f;
    }

    public void ReflectNodes()
    {
        foreach (var node in Nodes)
        {
            if(node.Min == 0 && node.Max == 360)
                continue;
            var min = node.Min;
            var max = node.Max;
            node.Max = 360 - min;
            node.Min = 360 - max;
        }
    }

    private void OnValidate() {
        foreach (var node in Nodes)
        {
            node.Min = Mathf.Clamp(node.Min, 0, 360);
            node.Max = Mathf.Clamp(node.Max, 0, 360);
        }
    }

    private void Start() {
        _NodeCache = new Dictionary<Transform, Node>(Nodes.Length);
        _Angles = new float[Nodes.Length];
        for (var i = 0; i < Nodes.Length; i++)
        {
            var node = Nodes[i];
            if (!_NodeCache.ContainsKey(node.Transform))
                _NodeCache.Add(node.Transform, node);
            _Angles[i] = node.Transform.rotation.z;
        }
    }

    public void UpdateRemotely() {
        if (!Application.isPlaying)
            Start();
        if (_NodeCache == null)
            return;
        if (Target == null || EndTransform == null)
            return;
        for (var i = 0; i < Iterations; i++)
        {
            CalculateIK();
        }
    }

    private void CalculateIK() {
        foreach (var node in Nodes)
        {
            RotateTowardsTarget(node.Transform);
        }
    }

    private void RotateTowardsTarget(Transform t) {
        Vector2 toTarget = Target.position - t.position;
        Vector2 toEnd = EndTransform.position - t.position;
        // Calculate how much we should rotate to get to the Target
        var angle = Vector3.SignedAngle(toEnd, toTarget, Vector3.back);
        // Flip sign if character is turned around
        //angle *= Mathf.Sign(t.root.localScale.x);
        // "Slows" down the IK solving
        angle *= Damping;
        // Wanted angle for rotation
        angle = -(angle - t.eulerAngles.z);
        // Take care of angle limits 
        if (_NodeCache.ContainsKey(t))
        {
            // Clamp angle in local space
            var node = _NodeCache[t];
            var parentRotation = t.parent ? t.parent.eulerAngles.z : 0;
            angle -= parentRotation;
            angle = ClampAngle(angle, node.Min, node.Max);
            angle += parentRotation;
        }
        t.rotation = Quaternion.Euler(0, 0, angle);
    }

    private static float ClampAngle(float angle, float min, float max) {
        angle = Mathf.Abs((angle % 360) + 360) % 360;
        return Mathf.Clamp(angle, min, max);
    }
}
