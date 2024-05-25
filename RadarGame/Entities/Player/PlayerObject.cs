using App.Engine;
using App.Engine.Template;
using Engine.graphics.Template;
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
        Console.WriteLine("Colision with " + ((IEntitie)colidedObject).Name);
    }

    public bool Static { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    public string Name { get; set; }
    
    public TexturedRectangle Spaceship { get; set; }
    private TextureAtlasRectangle Exhaust;
    private Polygon DebugPolygon { get; set; }
    private Polygon DebugPolygon2 { get; set; }
    private int bulletCount = 0;
    bool over1000 = false;
    private Vector2 lastPosition;
    private float lastRotation;
    private Random random = new Random();
    private float timer = 0;
    private Spaceship spaceship;
    
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
        CollisonShape = new List<Vector2>
        {
            new Vector2(-100, -100),
            new Vector2(100, -100),
            new Vector2(100, 100),
            new Vector2(-100, 100)
        };
        spaceship = new Spaceship();
        
        EntityManager.AddObject( new Camera(this));
        
        
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
       
        spaceship.Update(Position, Rotation, args);
        var t=  DrawSystem.DrawSystem.ScreenToWorldcord(mouseState.Position);
        spaceship.setCanonRotation( (float)Math.Atan2(t.Y - Position.Y, t.X - Position.X));
        
        if (timer > 0.1f)
        {
            Exhaust.setAtlasIndex(0, random.Next(0, 4));
            timer = 0;
        } 
        
        
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
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            spaceship.shoot();
        }
        
        spaceship.Accelerate(force);
        spaceship.Rotate(torque);
        PhysicsSystem.ApplyAngularForce( this, torque);
        Vector2 rotatedForce = new Vector2(
            force.X * (float)Math.Cos(Rotation) - force.Y * (float)Math.Sin(Rotation),
            force.X * (float)Math.Sin(Rotation) + force.Y * (float)Math.Cos(Rotation)
        );
        PhysicsSystem.ApplyForce(this, rotatedForce);





    }

    public void onDeleted()
    {
        Spaceship.Dispose();
    }

    public void Draw(List <View> surface)
    {
     spaceship.Draw(surface[1]);
    }
    
}