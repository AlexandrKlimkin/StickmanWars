using KlimLib.SignalBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDI;
using UnityEngine;

namespace Game.LevelSpecial.Railway {
    public class TrainTrafficLightsController : MonoBehaviour {
        [Dependency]
        private readonly SignalBus _SignalBus;

        public SpriteRenderer Sprite;
        public Color DefaultColor;
        public Color IsComingColor;

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<TrainMovementSignal>(OnTrainIsComingSignal, this);
        }

        private void SetTrainIsComing(bool isComing) {
            var color = isComing ? IsComingColor : DefaultColor;
            Sprite.color = color;
        }

        private void OnTrainIsComingSignal(TrainMovementSignal signal) {
            SetTrainIsComing(signal.Moving);
        }
    }
}
