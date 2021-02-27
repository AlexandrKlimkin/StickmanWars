using UnityEngine;

namespace Character.Control {
    public interface IAimViewProvider {
        AimViewBase AimPrefab { get; }
        Transform AimPositionPoint { get; }
    }
}
