using App.Engine;
using OpenTK.Mathematics;
using App.Engine.Template;
using RadarGame.PhysicsSystem;

namespace RadarGame.Entities;

public class GameObject : IEntitie, IPhysicsObject , IDrawObject
{
    public ColoredRectangle DebugColoredRectangle { get; set; }
    
    public PhysicsDataS PhysicsData { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    
    public GameObject()
    {
        PhysicsData = PhysicsData with { Acceleration = new Vector2(0,1) };  
        DebugColoredRectangle = new ColoredRectangle(new OpenTK.Mathematics.Vector2(0f, 0f), new OpenTK.Mathematics.Vector2(100f, 100f), OpenTK.Mathematics.Color4.Aqua);
    }


    public void Update(double deltaTime)
    {
        DebugColoredRectangle.drawInfo.Position = Position;
        DebugColoredRectangle.drawInfo.Rotation = Rotation;
    }

    public void Draw(View surface)
    {
        surface.Draw(DebugColoredRectangle);
    }
}