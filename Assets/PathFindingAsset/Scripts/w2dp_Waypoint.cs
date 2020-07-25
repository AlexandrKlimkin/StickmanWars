using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class w2dp_Waypoint : MonoBehaviour {
	
	public List <w2dp_Waypoint> 	Neighbours 		= new List<w2dp_Waypoint> ();

	public float 					F 				{ get { return G + H; }}
	public float 					G 				{ get; private set; }
	public float 					H 				{ get; private set; }
	public w2dp_Waypoint 			FromWaypoint 	{ get; private set; }
	public Vector3					Position		{ get { return this == null ? Vector3.zero : transform.position; }}


	/// <summary>
	/// Returns true if it successfully updates the FromWaypoint, H, G and F values of the Waypoint.
	/// </summary>
	public bool ProcessWaypoint (w2dp_Waypoint fromWaypoint, w2dp_Waypoint endWaypoint)
	{
		if (endWaypoint == null) 
			return false;
		
		FromWaypoint = fromWaypoint;
		H = Vector3.Distance (this.Position, endWaypoint.Position);
		G = fromWaypoint == null ? 0 : FromWaypoint.G + H;
		return true;
	}



	void OnEnable ()
	{
		w2dp_WaypointManager.RegisterWaypoint (this);
	}

	void OnDisable ()
	{
		w2dp_WaypointManager.UnregisterWaypoint (this);
	}

	void OnDrawGizmos ()
	{
		if (w2dp_WaypointManager.Instance.ShowNeighbours)
		{
			Gizmos.color = w2dp_WaypointManager.Instance.WaypointColor;
			Gizmos.DrawWireSphere(transform.position, w2dp_WaypointManager.Instance.GizmosSize);
			for (int i = 0; i < Neighbours.Count; i++)
			{
				Gizmos.DrawLine (Position, Neighbours[i].Position);
			}
		}
	}

	void OnDrawGizmosSelected ()
	{
		if (w2dp_WaypointManager.Instance.ShowNeighbours)
		{
			Gizmos.color = w2dp_WaypointManager.Instance.SelectedWaypointColor;
			Gizmos.DrawWireSphere(transform.position, w2dp_WaypointManager.Instance.GizmosSize);
			for (int i = 0; i < Neighbours.Count; i++)
			{
				Gizmos.DrawLine (Position, Neighbours[i].Position);
				Gizmos.DrawLine (Neighbours[i].Position, Position);
			}
		}
	}
}
