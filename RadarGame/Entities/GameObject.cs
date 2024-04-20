using App.Engine;
using OpenTK.Mathematics;
using App.Engine.Template;
using OpenTK.Windowing.Common;
using RadarGame.Physics;

namespace RadarGame.Entities;

public class GameObject : IEntitie, IPhysicsObject , IDrawObject
{
    public TexturedRectangle DebugColoredRectangle { get; set; }
    
    public PhysicsDataS PhysicsData { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    public string Name { get; set; }

    public GameObject( Vector2 position, float rotation, string name)
    {
        Position = position;
        Rotation = rotation;
        Name = name;
        Random random = new Random();
        PhysicsData = PhysicsData with { 
            Velocity = new Vector2((float)random.NextDouble()*100-50 , (float)random.NextDouble()*100-50),
           // Velocity = new Vector2(0 , 0), 
            Mass = 5f, 
            Drag = 0.01f,
            Acceleration = new Vector2(0f, 0f), 
            AngularAcceleration = 0f,
            AngularVelocity = (float)random.NextDouble()*10-5 };  
        DebugColoredRectangle = new TexturedRectangle(
            new OpenTK.Mathematics.Vector2(0f, 0f),
            new OpenTK.Mathematics.Vector2(200f, 200f), 
            new Texture("resources/cirno.png"),
            Name,
            true
            );
    }
    


    public void Update(FrameEventArgs args)
    {
        DebugColoredRectangle.drawInfo.Position = Position;
        DebugColoredRectangle.drawInfo.Rotation = Rotation;
    }

    public void Draw(View surface)
    {
        surface.Draw(DebugColoredRectangle);
    }
}