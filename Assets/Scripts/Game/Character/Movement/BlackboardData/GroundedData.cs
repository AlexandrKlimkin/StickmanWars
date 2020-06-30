namespace Character.Movement.Modules
{
    public class GroundedData : BlackboardData
    {
        public bool Grounded;
        public bool MainGrounded;
        public bool FallingDown;
        public float TimeFallingDown;
        public float TimeNotFallingDown;
        public float MinDistanceToGround;
        public float TimeSinceMainGrounded;
    }
}