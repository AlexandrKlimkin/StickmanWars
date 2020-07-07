using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Tools;
using Com.LuisPedroFonseca.ProCamera2D;
using Game.CameraTools;
using Game.LevelSpecial;
using KlimLib.SignalBus;
using UnityDI;
using UnityEditor;
using UnityEngine;

namespace Rendering
{
    public class TriggeredCameraTarget : MonoBehaviour, ICameraTarget
    {
        public List<UnitTrigger> Triggers;
        public float TriggerOutTime;
        public Vector3 Position => transform.position;
        public Vector3 Velocity => Vector3.zero;
        public float Direction => 0f;
        private float _TriggerOutTimer = 0f;

        private ProCamera2D _Camera;
        [Dependency]
        private readonly SignalBus _SignalBus;

        private void Start() {
            ContainerHolder.Container.BuildUp(this);
            _SignalBus.Subscribe<GameCameraSpawnedSignal>(OnCameraSpawned, this);
        }

        private void Update()
        {
            //if(_CameraBehaviour == null)
            //    return;
            //if (Triggers.Any(_ => _.ContainsUnit()))
            //{
            //    if (!_CameraBehaviour.Targets.Contains(this)) {
            //        _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, true));
            //    }
            //    _TriggerOutTimer = 0f;
            //}
            //else
            //{
            //    if (_CameraBehaviour.Targets.Contains(this)) {
            //        if (_TriggerOutTimer >= TriggerOutTime)
            //        {
            //            _SignalBus.FireSignal(new GameCameraTargetsChangeSignal(this, false));
            //        }
            //        else
            //        {
            //            _TriggerOutTimer += Time.deltaTime;
            //        }
            //    }
            //}
        }

        private void OnCameraSpawned(GameCameraSpawnedSignal signal) {
            _Camera = signal.Camera;
        }
    }
}