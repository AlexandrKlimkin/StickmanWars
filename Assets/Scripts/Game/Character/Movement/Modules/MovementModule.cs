using Character.Movement.Modules;

public abstract class MovementModule : IMovementModule
{
    protected Blackboard BB;
    protected CommonData CommonData;

    public MovementModule()
    {

    }
    public virtual void Initialize(Blackboard bb) {
        BB = bb;
        CommonData = BB.Get<CommonData>();
    }
    public abstract void Start();
    public abstract void Update();
    public abstract void FixedUpdate();
}