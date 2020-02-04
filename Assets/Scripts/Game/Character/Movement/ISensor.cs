namespace Character.Movement
{
    public interface ISensor
    {
        bool IsTouching { get; }
        float Distanse { get; }
    }
}