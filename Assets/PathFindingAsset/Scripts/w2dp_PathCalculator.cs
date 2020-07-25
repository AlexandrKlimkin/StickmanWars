using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class w2dp_PathCalculator {


	private List<w2dp_Waypoint> 		openList = new List<w2dp_Waypoint> ();
	private List<w2dp_Waypoint> 		closeList = new List<w2dp_Waypoint> ();
	private List<w2dp_Waypoint> 		finalPath = new List<w2dp_Waypoint> ();

	public w2dp_Waypoint 				StartWaypoint		{ get; private set; }
	public w2dp_Waypoint 				EndWaypoint			{ get; private set; }
	public w2dp_Waypoint 				CurrentWaypoint		{ get; private set; }

	
	/// <summary>
	/// Public constructor.
	/// </summary>
	public w2dp_PathCalculator () {}

	/// <summary>
	/// Returns the Waypoint that is nearest to the targetPosition.
	/// </summary>
	private static w2dp_Waypoint getNearestWaypoint (Vector3 targetPosition)
	{
		w2dp_Waypoint nearestWaypoint = null;
		float nearestDistance = float.PositiveInfinity;
		
		for (int i = 0; i < w2dp_WaypointManager.AllWaypoints.Count; i++)
		{
			if (w2dp_WaypointManager.AllWaypoints[i] != null)
			{
				float distance = Vector3.Distance (w2dp_WaypointManager.AllWaypoints[i].Position, targetPosition);
				if (distance < nearestDistance)
				{
					nearestWaypoint = w2dp_WaypointManager.AllWaypoints[i];
					nearestDistance = distance;
				}
			}
		}
		return nearestWaypoint;
	}

	/// <summary>
	/// Returns the shortest path from fromWaypoint to targetWaypoint.
	/// </summary>
	public List<w2dp_Waypoint> GetPath (w2dp_Waypoint fromWaypoint, w2dp_Waypoint targetWaypoint)
	{
		if (w2dp_WaypointManager.AllWaypoints == null || w2dp_WaypointManager.AllWaypoints.Count < 1)
			throw new UnityException ("Unable to generate path. Check Waypoints setup.");
		
		//reset lists
		openList = new List<w2dp_Waypoint> ();
		closeList = new List<w2dp_Waypoint> ();
		finalPath = new List<w2dp_Waypoint> ();
		
		//set start and end Waypoints
		StartWaypoint = fromWaypoint;
		EndWaypoint = targetWaypoint;
		CurrentWaypoint = StartWaypoint;
		
		if (StartWaypoint == EndWaypoint)
		{
			finalPath.Add (EndWaypoint);
			return finalPath;
		}
		else
		{
			StartWaypoint.ProcessWaypoint (null, EndWaypoint);
		}
		
		while (true)
		{
			//add CurrentWaypoint to closeList
			openList.Remove (CurrentWaypoint);
			closeList.Add (CurrentWaypoint);
			
			//add neighbours to open list
			bool thisWaypointHasNeighbours = false;
			for (int i = 0; i < CurrentWaypoint.Neighbours.Count; i++)
			{
				w2dp_Waypoint thisNeighbour = CurrentWaypoint.Neighbours[i];
				if (!closeList.Contains (thisNeighbour)) 
				{
					thisNeighbour.ProcessWaypoint (CurrentWaypoint, EndWaypoint);
					openList.Add (thisNeighbour);
					thisWaypointHasNeighbours = true;
				}
			}
			
			if (openList.Count < 1)
				throw new Exception ("Unable to calculate path as there are no more items in openList.");
			
			if (thisWaypointHasNeighbours)
			{
				//check for end Waypoint in open list
				for (int i = 0; i < openList.Count; i++)
				{
					if (openList[i] == EndWaypoint)
					{
						EndWaypoint.ProcessWaypoint (CurrentWaypoint, EndWaypoint);
						CurrentWaypoint = EndWaypoint;
						
						w2dp_Waypoint thisWaypoint = CurrentWaypoint;
						while (thisWaypoint.FromWaypoint != null)
						{
							finalPath.Insert (0, thisWaypoint);
							thisWaypoint = thisWaypoint.FromWaypoint;
						}
						return finalPath;
					}
				}
			}
			
			//set new currentWaypoint as nearest Waypoint from open list
			CurrentWaypoint = openList[0];
			for (int i = 1; i < openList.Count; i++)
			{
				if (openList[i].F < CurrentWaypoint.F) 
					CurrentWaypoint = openList[i];
			}
		}
	}

	/// <summary>
	/// Sets the start and end Waypoints based on currentAgentPosition and targetAgentPosition arguments, and returns the shortest path.
	/// </summary>
	public List<w2dp_Waypoint> GetPath (Vector3 currentAgentPosition, Vector3 targetAgentPosition)
	{
		return GetPath (getNearestWaypoint (currentAgentPosition), getNearestWaypoint (targetAgentPosition));
	}
}
