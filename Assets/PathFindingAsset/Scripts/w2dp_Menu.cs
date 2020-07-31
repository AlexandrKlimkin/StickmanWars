using UnityEngine;
using UnityEditor;
using System.Collections;

public class w2dp_Menu : MonoBehaviour {

	/// <summary>
	/// Create a new w2dp_Waypoint in the scene.
	/// </summary>
	//[MenuItem("GameObject/W2DP/w2dp_Waypoint", false, 10)]
	//static void NewW2dpWaypoint (MenuCommand menuCommand)
	//{
	//	//look for existing node manager and creates one if none found
	//	if (GameObject.FindObjectsOfType <w2dp_WaypointManager> ().Length < 1)
	//	{
	//		GameObject newManagerObj = new GameObject("w2dpWaypointManager");
	//		newManagerObj.AddComponent <w2dp_WaypointManager> ();
			
	//		GameObjectUtility.SetParentAndAlign(newManagerObj, menuCommand.context as GameObject);
	//		Undo.RegisterCreatedObjectUndo(newManagerObj, "Create " + newManagerObj.name);
	//	}

	//	//create new waypoint
	//	string newName = "";
	//	for (int i = 0; ; i++)
	//	{
	//		if (GameObject.Find(w2dp_WaypointManager._WaypointPrefix + i.ToString()) == null)
	//		{
	//			newName = w2dp_WaypointManager._WaypointPrefix + i.ToString();
	//			break;
	//		}
	//	}

	//	GameObject newObj = new GameObject(newName);
	//	newObj.AddComponent <w2dp_Waypoint> ();

	//	GameObjectUtility.SetParentAndAlign(newObj, menuCommand.context as GameObject);
	//	Undo.RegisterCreatedObjectUndo(newObj, "Create " + newName);
	//	Selection.activeObject = newObj;
	//}

	/// <summary>
	/// Create a new w2dp_WaypointManager in the scene.
	/// </summary>
	//[MenuItem("GameObject/W2DP/w2dp_WaypointManager", false, 10)]
	//static void NewW2dpWaypointManager (MenuCommand menuCommand)
	//{
	//	//look for existing node manager and creates one if none found
	//	if (GameObject.FindObjectsOfType <w2dp_WaypointManager> ().Length < 1)
	//	{
	//		GameObject newManagerObj = new GameObject("w2dpWaypointManager");
	//		newManagerObj.AddComponent <w2dp_WaypointManager> ();
			
	//		GameObjectUtility.SetParentAndAlign(newManagerObj, menuCommand.context as GameObject);
	//		Undo.RegisterCreatedObjectUndo(newManagerObj, "Create " + newManagerObj.name);
	//	}
	//}
}
