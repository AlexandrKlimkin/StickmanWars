using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class w2dp_WaypointManager : MonoBehaviour {

	public const string 	_WaypointPrefix 		= "waypoint";

	public Color32 			WaypointColor 			= Color.white;
	public Color32 			SelectedWaypointColor 	= Color.red;
	public bool 			ShowNeighbours 			= true;
	public float 			GizmosSize 				= 0.5f;
	public float 			AgentSpeed 				= 5f;

	public static w2dp_WaypointManager 	Instance 	{ get; private set; }
	public static List<w2dp_Waypoint>	AllWaypoints 	= new List<w2dp_Waypoint> ();


	public static void RegisterWaypoint (w2dp_Waypoint n)
	{
		if (w2dp_WaypointManager.AllWaypoints == null)
			w2dp_WaypointManager.AllWaypoints = new List<w2dp_Waypoint> ();
		
		if (!w2dp_WaypointManager.AllWaypoints.Contains (n))
			w2dp_WaypointManager.AllWaypoints.Add (n);
	}
	
	public static void UnregisterWaypoint (w2dp_Waypoint n)
	{
		if (w2dp_WaypointManager.AllWaypoints != null)
			w2dp_WaypointManager.AllWaypoints.Remove (n);
	}


	void OnEnable ()
	{
		Instance = this;
	}

	void OnDestroy ()
	{
		Instance = null;
	}
}
