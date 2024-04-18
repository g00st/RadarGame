using OpenTK.Mathematics;

namespace RadarGame.PhysicsSystem;

public interface IPhysicsObject
{
    PhysicsData PhysicsData { get; set; }
    Vector2 Position { get; set; }
    Vector2 Center { get; set; }
    float Rotation { get; set; }
}

public struct PhysicsData
{
    float Mass { get; set; }
    public float Drag { get; set; }
    public float Velocity { get; set; }
    public float Acceleration { get; set; }
    public float AngularVelocity { get; set; }
    public float AngularAcceleration { get; set; }
}