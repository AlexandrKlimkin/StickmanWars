using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// Pathfinding script that controls a single agent.
/// </summary>
public class w2dp_Agent : MonoBehaviour {

	public enum State
	{
		Idling,
		CalculatingPath,
		Moving,
	}
	
	public event Action				OnStartCalculatePath;
	public event Action				OnAgentMove;
	public event Action				OnAgentCompleteMove;

	public float					AgentSpeed				= 8f;
	public float					DistanceTolerance 		= 0.2f;

	private w2dp_PathCalculator		calculator 				= new w2dp_PathCalculator ();
	private State 					currentState;
	private Vector3					startPosition;
	private Vector3					nextPosition;
	private int						positionIndex;
	private float					duration;
	private float					timeElapsed;
	private List <w2dp_Waypoint>	currentPath;


	private void startMoving (Vector3 fromPosition, Vector3 toPosition)
	{
		startPosition = fromPosition;
		nextPosition = toPosition;
		timeElapsed = 0;
		duration = Vector3.Distance (startPosition, nextPosition) / AgentSpeed;
	}

	private void updateMoving ()
	{
		if (Vector3.Distance (transform.position, nextPosition) > DistanceTolerance)
		{
			transform.position = Vector3.Lerp (startPosition, nextPosition, timeElapsed / duration);
			timeElapsed += Time.deltaTime;
		}
		else
		{
			if (positionIndex >= currentPath.Count) 
			{
				endMove ();
			}
			else
			{
				startMoving (transform.position, currentPath[positionIndex].Position);
				positionIndex++;
			}
		}
	}

	private void endMove ()
	{
		if (OnAgentCompleteMove != null)
			OnAgentCompleteMove ();

		currentState = State.Idling;
	}


	/// <summary>
	/// Function that moves the agent via the shortest path from one Waypoint to another.
	/// </summary>
	public void Move (w2dp_Waypoint fromWaypoint, w2dp_Waypoint targetWaypoint)
	{
		currentState = State.Idling;
		
		if (OnStartCalculatePath != null)
			OnStartCalculatePath ();
		
		currentState = State.CalculatingPath;
		currentPath = calculator.GetPath (fromWaypoint, targetWaypoint);
		
		if (currentPath != null)
		{
			positionIndex = 0;
			startMoving (transform.position, fromWaypoint.Position);
			
			if (OnAgentMove != null)
				OnAgentMove ();
			
			currentState = State.Moving;
		}
	}

	/// <summary>
	/// Function that moves the agent via the shortest path from current position to the waypoint closest to the end position.
	/// </summary>
	public void Move (Vector3 fromPosition, Vector3 toPosition)
	{
		currentState = State.Idling;

		if (OnStartCalculatePath != null)
			OnStartCalculatePath ();
		
		currentState = State.CalculatingPath;
		currentPath = calculator.GetPath (fromPosition, toPosition);
		
		if (currentPath != null)
		{
			positionIndex = 0;
			startMoving (transform.position, fromPosition);
			
			if (OnAgentMove != null)
				OnAgentMove ();
			
			currentState = State.Moving;
		}
	}

	void Update () {

		switch (currentState)
		{
		case State.Moving: 	updateMoving (); break;
		default: 			break;
		}
	}
}
