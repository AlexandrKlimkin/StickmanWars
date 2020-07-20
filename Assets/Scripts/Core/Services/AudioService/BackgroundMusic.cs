using Core.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityDI;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour {
    public string BackgroundTrackName;
    [Dependency]
    private readonly AudioService _AudioService;

    public AudioEffect AudioTrack;

    private void Start() {
        ContainerHolder.Container.BuildUp(this);
        AudioTrack = _AudioService.PlayMusic(BackgroundTrackName);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.P)) {
            AudioTrack.gameObject.SetActive(true);
            AudioTrack.Play(false);
        }
    }

}
