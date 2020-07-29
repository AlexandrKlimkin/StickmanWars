using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor (typeof (w2dp_Waypoint))]
[CanEditMultipleObjects]

public class w2dp_WaypointEditor : Editor {

	private SerializedObject 	m_waypoint;
	private SerializedProperty 	m_neighbours;
	private w2dp_Waypoint 		thisWaypoint;

	

	public void OnEnable()
	{
		m_waypoint = new SerializedObject (target);
		m_neighbours = m_waypoint.FindProperty ("Neighbours");
		thisWaypoint = (w2dp_Waypoint) target;
	}
	
	
	public override void OnInspectorGUI()
	{
		m_waypoint.UpdateIfDirtyOrScript();

		//LABEL
		GUILayout.Label("Neighbours", EditorStyles.boldLabel);

		GUI.color = Color.cyan;
		EditorGUILayout.HelpBox("* Select exactly one Waypoint to add new neighbour Waypoint to it.\n" +
		                        "* Select exactly two Waypoints to insert a new Waypoint in between.\n" +
		                        "* Select more than one Waypoint to link the selected Waypoints together."
		                        , MessageType.None, true);
		GUI.color = Color.white;
		if (targets.Length == 1)
		{
			//SHOW CONNECTED NEIGHBOURS
			for (int i = 0; i < m_neighbours.arraySize; i++)
			{
				GUILayout.BeginHorizontal();
				
				EditorGUILayout.ObjectField (m_neighbours.GetArrayElementAtIndex(i).objectReferenceValue as w2dp_Waypoint, typeof(w2dp_Waypoint), true); 
				
				if (GUILayout.Button("-", GUILayout.Width(20)))
				{
					DeleteThisNeighbour (i);
				}
				
				GUILayout.EndHorizontal();
			}

			//BUTTON TO ADD NEW NEIGHBOUR
			if (GUILayout.Button("Add New Neighbour"))
			{
				GameObject newObj = Instantiate(thisWaypoint.gameObject) as GameObject;
				if (newObj)
				{
					for (int i = 0; ; i++)
					{
						if (GameObject.Find(w2dp_WaypointManager._WaypointPrefix + i.ToString()) == null)
						{
							newObj.name = w2dp_WaypointManager._WaypointPrefix + i.ToString();
							Undo.RegisterCreatedObjectUndo (newObj, newObj.name);
							break;
						}
					}
					w2dp_Waypoint newWaypoint = newObj.GetComponent<w2dp_Waypoint>();
					newWaypoint.Neighbours.Clear ();
					AddNewNeighbour (newWaypoint);
					Selection.activeTransform = newWaypoint.transform;
				}
			}
		}

		//BUTTON TO LINK 2 NEIGHBOURS
		if (targets.Length > 1)
		{
			if (GUILayout.Button("Link Selected Neighbours"))
			{
				foreach (Object o in targets)
				{
					w2dp_Waypoint currentWaypoint = (w2dp_Waypoint)o;

					for (int i = 0; i < targets.Length; i++)
					{
						if (targets[i].name != o.name)
						{
							w2dp_Waypoint currentNeighbour = (w2dp_Waypoint) targets[i];
							List<w2dp_Waypoint> n = currentNeighbour.Neighbours;
							if (n.IndexOf(currentWaypoint) < 0)
							{
								currentNeighbour.Neighbours.Add (currentWaypoint);
							}
						}
					}
				}
			}
		}
		
		if (targets.Length == 2)
		{
			if (GUILayout.Button("Insert In Between Selected Neighbours"))
			{
				w2dp_Waypoint waypoint1 = (w2dp_Waypoint) targets[0];
				w2dp_Waypoint waypoint2 = (w2dp_Waypoint) targets[1];

				GameObject newObj = Instantiate (waypoint1.gameObject) as GameObject;
				w2dp_Waypoint currentWaypoint = newObj.GetComponent<w2dp_Waypoint> ();

				currentWaypoint.Neighbours.Clear();
				currentWaypoint.Neighbours.Add(waypoint1);
				currentWaypoint.Neighbours.Add(waypoint2);

				waypoint1.Neighbours.Add (currentWaypoint);
				waypoint1.Neighbours.Remove (waypoint2);

				waypoint2.Neighbours.Add (currentWaypoint);
				waypoint2.Neighbours.Remove (waypoint1);

				currentWaypoint.transform.position = (waypoint1.transform.position + waypoint2.transform.position) / 2;

				for (int i = 0; ; i++)
				{
					if (GameObject.Find(w2dp_WaypointManager._WaypointPrefix + i.ToString()) == null)
					{
						currentWaypoint.transform.name = w2dp_WaypointManager._WaypointPrefix + i.ToString();
						break;
					}
				}
				
				Selection.activeTransform = currentWaypoint.transform;
			}
		}
		
		EditorGUILayout.HelpBox("If you delete any Waypoints using the Delete key on your keyboard, the Undo step will constitute broken links to Neighbours.",
		                        MessageType.Info, true);

		m_waypoint.ApplyModifiedProperties();
	}
	
	
	void OnDestroy ()
	{
		foreach (Object t in targets)
		{
			w2dp_Waypoint currentWaypoint = (w2dp_Waypoint) t;
			if (currentWaypoint == null) 
			{
				for (int i = 0; i < currentWaypoint.Neighbours.Count; i++)
				{
					if (currentWaypoint.Neighbours[i] != null)
						currentWaypoint.Neighbours[i].Neighbours.Remove (currentWaypoint);
				}
			}
		}
	}

	/// <summary>
	/// Adds a new neighbout n to thisWaypoint, and adds thisWaypoint as a new neighbour to n.
	/// </summary>
	/// <param name="n">N.</param>
	void AddNewNeighbour (w2dp_Waypoint n)
	{
		m_neighbours.arraySize++;
		m_neighbours.GetArrayElementAtIndex (m_neighbours.arraySize-1).objectReferenceValue = n;

		SerializedObject m_N_waypoint = new SerializedObject (n);
		SerializedProperty m_N_neighbours = m_N_waypoint.FindProperty ("Neighbours");
		m_N_neighbours.arraySize++;
		m_N_neighbours.GetArrayElementAtIndex (m_N_neighbours.arraySize-1).objectReferenceValue = thisWaypoint;
		m_N_waypoint.ApplyModifiedProperties();
	}
	
	/// <summary>
	/// Deleting neighbour Waypoints in serialised way so that undo can be done.
	/// </summary>
	void DeleteThisNeighbour (int index)
	{
		//delete this Waypoint as neighbour of indexed Waypoint
		w2dp_Waypoint waypointToDelete = m_neighbours.GetArrayElementAtIndex (index).objectReferenceValue as w2dp_Waypoint;
		bool isNeighbourFound = false;

		if (waypointToDelete != null)
		{
			SerializedObject m_N_waypoint = new SerializedObject (waypointToDelete);
			SerializedProperty m_N_neighbours = m_N_waypoint.FindProperty ("Neighbours");
			for (int i = 0; i < m_N_neighbours.arraySize; i++)
			{
				w2dp_Waypoint currentWaypoint = m_N_neighbours.GetArrayElementAtIndex (i).objectReferenceValue as w2dp_Waypoint;
				if (!isNeighbourFound)
				{
					if (currentWaypoint == m_waypoint.targetObject as w2dp_Waypoint)
						isNeighbourFound = true;
					else
						continue;
				}
				
				if (isNeighbourFound)
				{
					//replace this Waypoint with next Waypoint
					if (i+1 < m_N_neighbours.arraySize)
					{
						w2dp_Waypoint nextWaypoint = m_N_neighbours.GetArrayElementAtIndex (i+1).objectReferenceValue as w2dp_Waypoint;
						m_N_neighbours.GetArrayElementAtIndex (i).objectReferenceValue = nextWaypoint;
					}
				}
			}
			m_N_neighbours.arraySize--;
			m_N_waypoint.ApplyModifiedProperties();
		}

		//shift array of neighbour Waypoints (after neighbourToDelete) up to fill the gap left by neighbourToDelete
		for (int i = index; i < m_neighbours.arraySize-1; i++)
		{
			w2dp_Waypoint nextWaypoint = m_neighbours.GetArrayElementAtIndex (i+1).objectReferenceValue as w2dp_Waypoint;
			m_neighbours.GetArrayElementAtIndex (i).objectReferenceValue = nextWaypoint;
		}
		m_neighbours.arraySize--;
		m_waypoint.ApplyModifiedProperties ();
	}
}
