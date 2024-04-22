using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Physics;

namespace RadarGame.Entities;

public class PlayerObject : IEntitie, IPhysicsObject, IDrawObject , IColisionObject
{
    public PhysicsDataS PhysicsData { get; set; }
    public List<Vector2> CollisonShape { get; set; }
    public void OnColision(IColisionObject colidedObject)
    {
        Console.WriteLine("Colision with " + colidedObject);
    }

    public bool Static { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    public string Name { get; set; }
    
    public TexturedRectangle DebugColoredRectangle { get; set; }
    private int bulletCount = 0;
    bool over1000 = false;
    private Vector2 lastPosition;
    private float lastRotation;
    private Random random = new Random();
    private float timer = 0;
    
    public PlayerObject(Vector2 position, float rotation, string name = "Player")
    {
        Static = false;
        Name = name;
        Position = position;
        Rotation = rotation;
        PhysicsData = new PhysicsDataS
        {
            Velocity = new Vector2(0, 0),
            Mass = 5f,
            Drag = 0.01f,
            Acceleration = new Vector2(0f, 0f),
            AngularAcceleration = 0f,
            AngularVelocity = 0f
        };
        DebugColoredRectangle = new TexturedRectangle(
            new OpenTK.Mathematics.Vector2(0f, 0f),
            new OpenTK.Mathematics.Vector2(200f, 200f),
            new Texture("resources/cirno.png"),
            Name,
            true
        );
        CollisonShape = new List<Vector2>
        {
            new Vector2(-100, -100),
            new Vector2(100, -100),
            new Vector2(100, 100),
            new Vector2(-100, 100)
        };
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState)
    {
        timer += (float)args.Time;
        
        lastPosition = Position;
        lastRotation = Rotation;
        DebugColoredRectangle.drawInfo.Position = Position;
        DebugColoredRectangle.drawInfo.Rotation = Rotation;
        Vector2 force = new Vector2(0, 0);
        float   torque = 0;
        
        
        if (keyboardState.IsKeyDown(Keys.W))
        {
            force += new Vector2(0f, 100f);
        }
        if (keyboardState.IsKeyDown(Keys.A))
        {
           force += new Vector2(-100f, 0f);
        }
        if (keyboardState.IsKeyDown(Keys.S))
        {
            force += new Vector2(0f, -100f);
        }
        if (keyboardState.IsKeyDown(Keys.D))
        {
          force += new Vector2(100f, 0f);
        }
        if (keyboardState.IsKeyDown(Keys.E))
        {
            torque -= 3f;
        }
        if (keyboardState.IsKeyDown(Keys.Q))
        {
            torque += 3f;
        }
        if (keyboardState.IsKeyDown(Keys.LeftShift))
        {
            force += new Vector2(0, 1000);
        }

        if (keyboardState.IsKeyDown(Keys.Space) && timer > 0.1f)
        {
            
            timer = 0;
        
        
            string name = "Bullet"+bulletCount;
                
            Vector2 bulletvell = new Vector2(
                0 * (float)Math.Cos(Rotation) - 1000 * (float)Math.Sin(Rotation),
                0 * (float)Math.Sin(Rotation) + 1000 * (float)Math.Cos(Rotation)
            );
            
            EntityManager.AddObject(new GameObject(Position,Rotation,name, bulletvell,0) );
            bulletCount++;
            if (bulletCount > 1000)
            {
                over1000 = true;
                bulletCount = 0;
            }
            if (over1000)
            {
                Console.WriteLine("Bullet"+bulletCount + " removed in Player" );
                EntityManager.RemoveObject(EntityManager.GetObject("Bullet"+bulletCount));
            }
        }
        
        
        PhysicsSystem.ApplyAngularForce( this, torque);
        
        //rotate by Rotation
        Vector2 rotatedForce = new Vector2(
            force.X * (float)Math.Cos(Rotation) - force.Y * (float)Math.Sin(Rotation),
            force.X * (float)Math.Sin(Rotation) + force.Y * (float)Math.Cos(Rotation)
        );
        
        PhysicsSystem.ApplyForce(this, rotatedForce);





    }

    public void onDeleted()
    {
        DebugColoredRectangle.Dispose();
    }

    public void Draw(View surface)
    {
     //   surface.rotation = - lastRotation;
       // Console.WriteLine(surface.rotation);
        surface.vpossition = lastPosition;
        surface.vsize = new Vector2(1920/1.5f  + Math.Abs(PhysicsData.Velocity.Length*5) , 1080/1.5f + Math.Abs(PhysicsData.Velocity.Length*5));
        surface.Draw(DebugColoredRectangle);
    }
    
}