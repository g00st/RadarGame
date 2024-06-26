using App.Engine;
using OpenTK.Mathematics;
using App.Engine.Template;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Physics;
using RadarGame.Radarsystem;

namespace RadarGame.Entities;

public class GameObject : IEntitie, IPhysicsObject , IDrawObject, IColisionObject, IRadarObject
{
    //--------------------------------------------------------------------------------
    // THis is a simple game object that can be used to test the Project and Systems
    //--------------------------------------------------------------------------------
    public TexturedRectangle DebugColoredRectangle { get; set; }
    
    public PhysicsDataS PhysicsData { get; set; }
    public List<Vector2> CollisonShape { get; set; }
    public void OnColision(IColisionObject colidedObject)
    {
        if (((IEntitie)colidedObject).Name.Contains("Bullet"))
        {
            Console.WriteLine("Colision with " + colidedObject);
            EntityManager.RemoveObject((IEntitie)colidedObject);
            EntityManager.RemoveObject(this);
        }
        else
        {


            IPhysicsObject physicsObject = (IPhysicsObject)colidedObject;
            var differencevector = physicsObject.Position - Position;
            PhysicsSystem.ApplyForce(this, -differencevector * 100);
        }
    }
    private Polygon DebugPolygon = Polygon.Circle( new Vector2(0, 0), 50, 100,new SimpleColorShader(Color4.Ivory), "SDF", true);
    

    public bool Static { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    public float RadarSdf(Vector2 Position)
    {
       //use circular sdf
        return (Position - this.Position).Length - 50;
    }

    public string Name { get; set; }

    public GameObject( Vector2 position, float rotation, string name)
    {
        Position = position;
        Rotation = rotation;
        Static = false;
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
        CollisonShape = new List<Vector2>
        {
            new Vector2(-50, -50),
            new Vector2(50, -50),
            new Vector2(50, 50),
            new Vector2(-50, 50)
        };
    }
    public GameObject  (Vector2 position, float rotation, string name, Vector2 vel , float angVel)
    {
        Static = false;
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
        CollisonShape = new List<Vector2>
        {
            new Vector2(-25, -25),
            new Vector2(25, -25),
            new Vector2(25, 25),
            new Vector2(-25, 25)
        };
    }
    


    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        DebugColoredRectangle.drawInfo.Position = Position;
        DebugColoredRectangle.drawInfo.Rotation = Rotation;
        DebugPolygon.Position = Position;
        DebugPolygon.Rotation = Rotation;
    }

    public void onDeleted()
    {
        Console.WriteLine("Deleted");
        DebugColoredRectangle.Dispose();
    }


    public void Draw(View surface)
    {
        surface.Draw(DebugColoredRectangle);
        surface.Draw(DebugPolygon);
    }
}