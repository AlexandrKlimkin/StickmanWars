namespace Stickman.Movement
{
    public interface IGroundSensor
    {
        bool IsGrounded { get; }
        float DistanseToGround { get; }
    }
}