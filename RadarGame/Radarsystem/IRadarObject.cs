using OpenTK.Mathematics;

namespace RadarGame.Radarsystem;

public interface IRadarObject
{
    public Vector2 Position { get; }
    public string Name { get; }
    public float Rotation { get; }
    public float RadarSdf(Vector2 Position);
}