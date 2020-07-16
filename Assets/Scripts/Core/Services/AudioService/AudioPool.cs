using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Core.Audio {
    public sealed class AudioPool : MonoBehaviour {
        private static readonly List<AudioEffect> _Pool = new List<AudioEffect>();
        [Dependency]
        private readonly AudioService _AudioService;

        public void Clear() {
            _Pool.Clear();
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        private void Awake() {
            ContainerHolder.Container.BuildUp(GetType(), this);
        }

        public AudioEffect GetTrack(string trackName, bool forceSingle) {
            AudioEffect effect = null;
            var tracks = _Pool.Where(_ => _.Name == trackName);
            if (forceSingle)
                effect = tracks.FirstOrDefault();
            else
                effect = tracks.FirstOrDefault(_ => !_.gameObject.activeSelf);
            if (effect == null)
                effect = AddTrack(trackName);
            if (effect.gameObject.activeSelf)
                effect.gameObject.SetActive(false);
            effect.gameObject.SetActive(true);
            if (effect.IsPlaying)
                effect.StopAudio();
            return effect;
        }

        public bool RemoveFromPool(AudioEffect audioEffect) {
            return _Pool.Remove(audioEffect);
        }

        private AudioEffect AddTrack(string trackName) {
            var trackPrefab = _AudioService.GetTrack(trackName);
            var track = Instantiate(trackPrefab, transform);
            track.name = Utils.RemoveClonePostfix(track.name);
            _Pool.Add(track);
            return track;
        }
    }
}
