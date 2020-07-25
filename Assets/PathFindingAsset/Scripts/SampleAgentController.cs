using UnityEngine;
using System.Collections;

public class SampleAgentController : MonoBehaviour {

	public Camera		RenderCamera;
	public w2dp_Agent	Agent;

	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButton (0))
		{
			Agent.Move (transform.position, RenderCamera.ScreenToWorldPoint (Input.mousePosition));
		}
	}
}
