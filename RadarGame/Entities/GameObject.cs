using App.Engine;
using OpenTK.Mathematics;
using App.Engine.Template;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
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
            new OpenTK.Mathematics.Vector2(100f, 100f), 
            new Texture("resources/cirno.png"),
            Name,
            true
            );
    }
    public GameObject  (Vector2 position, float rotation, string name, Vector2 vel , float angVel)
    {
        Position = position;
        Rotation = rotation;
        Name = name;
        PhysicsData = PhysicsData with { 
            Velocity = vel, 
            Mass = 1f, 
            Drag = 0.00f,
            Acceleration = new Vector2(0f, 0f), 
            AngularAcceleration = 0f,
            AngularVelocity = angVel };  
        DebugColoredRectangle = new TexturedRectangle(
            position,
            new OpenTK.Mathematics.Vector2(50f, 50f), 
            new Texture("resources/cirno.png"),
            Name,
            true
            );
        DebugColoredRectangle.drawInfo.Rotation = rotation;
    }
    


    public void Update(FrameEventArgs args, KeyboardState keyboardState)
    {
        DebugColoredRectangle.drawInfo.Position = Position;
        DebugColoredRectangle.drawInfo.Rotation = Rotation;
    }

    public void onDeleted()
    {
        Console.WriteLine("Deleted");
        DebugColoredRectangle.Dispose();
    }


    public void Draw(View surface)
    {
        surface.Draw(DebugColoredRectangle);
    }
}