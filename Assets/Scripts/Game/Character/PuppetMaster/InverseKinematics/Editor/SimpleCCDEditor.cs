﻿using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class SimpleCCDEditor
{
	static SimpleCCDEditor ()
	{
		SceneView.onSceneGUIDelegate += OnScene;
	}

	// Scales scene view gizmo, feel free to change ;)
	const float gizmoSize = 0.5f;

	static void OnScene(SceneView sceneview)
	{
		var targets = GameObject.FindObjectsOfType<SimpleCCD>();

		foreach (var target in targets)
		{
#if UNITY_EDITOR
            if(!target.DrawAnglesGizmos)
                continue;
#endif
            foreach (var node in target.Nodes)
			{
				if (node.Transform == null)
					continue;

				Transform transform = node.Transform;
				Vector3 position = transform.position;

				float handleSize = HandleUtility.GetHandleSize(position);
				float discSize = handleSize * gizmoSize;


				float parentRotation = transform.parent ? transform.parent.eulerAngles.z : 0;
				Vector3 min = Quaternion.Euler(0, 0, node.Min + parentRotation)*Vector3.down;
				Vector3 max = Quaternion.Euler(0, 0, node.Max + parentRotation)*Vector3.down;
  
				Handles.color = new Color(0, 1, 0, 0.1f);
				Handles.DrawWireDisc(position, Vector3.back, discSize);
				Handles.DrawSolidArc(position, Vector3.forward, min, node.Max - node.Min, discSize);

				Handles.color = Color.green;
				Handles.DrawLine(position, position + min * discSize);
				Handles.DrawLine(position, position + max*discSize);

				Vector3 toChild = FindChildNode(transform, target.EndTransform).position - position;
				Handles.DrawLine(position, position + toChild);
			}
		}
	}

	static Transform FindChildNode (Transform parent, Transform endTransform)
	{
		if (endTransform.parent != parent)
			return FindChildNode(parent, endTransform.parent); ;

		return endTransform;
	}
}