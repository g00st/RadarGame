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
        Console.WriteLine("Colision with " + ((IEntitie)colidedObject).Name);
    }

    public bool Static { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    public string Name { get; set; }
    
    public TexturedRectangle DebugColoredRectangle { get; set; }
    private Polygon DebugPolygon { get; set; }
    private Polygon DebugPolygon2 { get; set; }
    private int bulletCount = 0;
    bool over1000 = false;
    private Vector2 lastPosition;
    private float lastRotation;
    private Random random = new Random();
    private float timer = 0;
    // static string filepath = "resources/Sounds/laser-gun.wav";  // Audio Corruption
    
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
        
        DebugPolygon = Polygon.Circle(this.Position,10,50,new SimpleColorShader(Color4.Red),"DebugPolygon",true);
        DebugPolygon2 = Polygon.Rectangle( this.Position, new Vector2(200,200),0, new SimpleColorShader(Color4.Azure),"DebugPolygon2",true);
       // DebugPolygon = Polygon.Rectangle(this.Position, new  Vector2(200,200),0, new SimpleColorShader(Color4.Azure),"DebugPolygon",true);
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        timer += (float)args.Time;
        lastPosition = Position;
        lastRotation = Rotation;
        DebugColoredRectangle.drawInfo.Position = Position;
        DebugColoredRectangle.drawInfo.Rotation = Rotation;
        DebugPolygon.Position = Position;
        DebugPolygon.Rotation = Rotation;
        DebugPolygon2.Position = Position;
        DebugPolygon2.Rotation = Rotation;
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
            //TODO: add sound effect
            EntityManager.AddObject(new GameObject(Position,Rotation,name, bulletvell,0) );
            bulletCount++;
            if (bulletCount > 1000)
            {
                over1000 = true;
                bulletCount = 0;
            } else
            {
                // SoundSystem.SoundSystem.PlayThisTrack(filepath, 2);  // Audio Corruption
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

    public void Draw(List <View> surface)
    {
      //  surface.rotation = - lastRotation;
       surface[0].vpossition = new Vector2(Position.X, Position.Y);
       // Console.WriteLine(surface.rotation);
       surface[0].vsize = new Vector2(1920 , 1080);
      surface[0].Draw(DebugColoredRectangle);
      surface[0].Draw(DebugPolygon2);
        Vector2 last = this.Position;
        
    }
    
}