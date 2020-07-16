using Character.Movement;
using Character.Movement.Modules;
using UnityDI;

public abstract class MovementModule : IMovementModule
{
    protected Blackboard BB;
    protected CommonData CommonData;

    public virtual void Initialize(Blackboard bb) {
        BB = bb;
        CommonData = BB.Get<CommonData>();
    }

    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void LateUpdate() { }
    public virtual void FixedUpdate() { }
}