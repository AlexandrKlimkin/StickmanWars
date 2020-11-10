using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrafficLightController : MonoBehaviour {

    public enum TrafficLightControllerState { On, Off }

    public TrafficLightControllerState State { get; private set; }

    public Transform States;

    public void SetState(TrafficLightControllerState state) {
        if (state == State)
            return;
        State = state;
        for (int i = 0; i < States.childCount; i++) {
            var child = States.GetChild(i);
            child.gameObject.SetActive(child.name == state.ToString());
        }
    }
}
