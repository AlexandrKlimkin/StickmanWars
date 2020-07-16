using Core.Services;
using KlimLib.ResourceLoader;
using KlimLib.SignalBus;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Core.Audio {
    public class AudioService : ILoadableService {
        [Dependency]
        private readonly IResourceLoaderService _ResourceLoaderService;
        [Dependency]
        private readonly SignalBus _SignalBus;

        public AudioPool AudioPool { get; private set; }

        public Transform PoolTransform => AudioPool.transform;

        public float SFXMaxDistanceMultiplier { get; private set; } = 1f;
        public float SFXVolumeMultiplier { get; private set; } = 1f;
        public float MusicVolumeMultiplier { get; private set; } = 1f;

        public AudioEffect PlayMusic(string trackName) {
            return PlaySound2D(trackName, true, true);
        }

        public AudioEffect PlaySound2D(string trackName, bool forceSingle, bool rise) {
            return PlaySound3D(trackName, forceSingle, rise, Vector3.zero);
        }

        public AudioEffect PlaySound3D(string trackName, bool forceSingle, bool rise, Vector3 position) {
            if (!CanPlayTrack(trackName))
                return null;
            var track = AudioPool.GetTrack(trackName, forceSingle);
            track.transform.position = position;
            track.Play(rise);
            return track;
        }

        public AudioEffect GetTrack(string trackName) {
            var track = _ResourceLoaderService.LoadResource<AudioEffect>("Prefabs/AudioTracks/" + trackName);
            if (track == null)
                Debug.LogError($"there is no Prefabs/AudioTracks/{trackName} track");
            return _ResourceLoaderService.LoadResource<AudioEffect>("Prefabs/AudioTracks/" + trackName);
        }

        public bool CanPlayTrack(string trackName) {
            var audioGroup = GetTrack(trackName).AudioGroup;
            switch (audioGroup) {
                case AudioGroup.SFX:
                    return true;
                case AudioGroup.Music:
                    return true;
            }
            return true;
        }

        public void SetSFXMaxDistanceMultiplier(float mult) {
            SFXMaxDistanceMultiplier = mult;
        }

        public void ResetSFXMaxDistanceMultiplier() {
            SFXMaxDistanceMultiplier = 1f;
        }

        void ILoadableService.Load() {
            var audioPoolGameObject = new GameObject(typeof(AudioPool).Name, typeof(AudioPool)); //todo: move to task
            Object.DontDestroyOnLoad(audioPoolGameObject);
            AudioPool = audioPoolGameObject.GetComponent<AudioPool>();
        }
    }
}
