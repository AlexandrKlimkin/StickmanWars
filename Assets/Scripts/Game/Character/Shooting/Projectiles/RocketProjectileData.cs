using Character.Shooting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Character.Shooting {
    public class RocketProjectileData : BulletProjectileData {
        public float ActivationTime;
        public float SpeedBeforeActivation;
    }
}
