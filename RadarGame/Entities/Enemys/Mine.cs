using App.Engine;
using App.Engine.Template;
using Engine.graphics.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Physics;

namespace RadarGame.Entities.Enemys;

public class Mine : IEnemie, IDrawObject, IColisionObject, IcanBeHurt
{
    public PhysicsDataS PhysicsData { get; set; }
    public List<Vector2> CollisonShape { get; set; }
    public void OnColision(IColisionObject colidedObject)
    {
      explode();
    }

    public bool Static { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    static int id = 0;
    public float Rotation { get; set; }
    private PlayerObject target;
    private float Explosiontimer = 0;
    private float Explosiontime = 1f;
    private float blinkSpeed = 0.01f;
    private bool isDead = false;
    private bool ligth = false;
    private float explosiondistance = 500;
    public string Name { get; set; }
    private TextureAtlasRectangle texture = new TextureAtlasRectangle(  new Vector2(0,0), new Vector2(    100,100), new Vector2(1,2), new Texture("resources/Enemies/Mine.png"), "Mine");
    
    public Mine(Vector2 position, EnemyManager entityManager)
    {
        Position = position;
        EnemyManager = entityManager;
        Name = "Mine" + id++;
        PlayerObject target = (PlayerObject)EntityManager.GetObject("Player");
        PhysicsData = new PhysicsDataS
        {
            Velocity = Vector2.Zero,
            Mass = 1f,
            Drag = 0.000f,
            Acceleration = Vector2.Zero,
            AngularAcceleration = 0f,
            AngularVelocity = 0.5f
        };
        
        Static = false;
        CollisonShape = new List<Vector2>
        {
            new Vector2(-50, -50),
            new Vector2(50, -50),
            new Vector2(50, 50),
            new Vector2(-50, 50)
        };
    }
    
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        
        if (target == null)
        {
            target = (PlayerObject)EntityManager.GetObject("Player");
            return;
        }
        
        var distance = (target.Position - Position).Length;
        
        if (distance < explosiondistance)
        {
            Explosiontimer += (float)args.Time ;
            if (Explosiontimer > Explosiontime)
            {
                explode();
            }
            if (Explosiontimer % blinkSpeed < blinkSpeed / 2)
            {
                ligth = true;
            }
            else
            {
                ligth = false;
            }
        }
        else
        {
            Explosiontimer = 0;
            blinkSpeed = 0.1f;
        }

      
        
        
    }

    private void explode()
    {
        if(IsDead()) return;
        isDead = true;
         AnimatedExposion.newExplosion(Position, explosiondistance*2);
         foreach (var colisionObject in ColisionSystem.getinRadius( Position, explosiondistance, false,true))
         {
             if (colisionObject is IcanBeHurt)
             {
                 ((IcanBeHurt)colisionObject).applyDamage( (int) (50f * (1 - (Position - colisionObject.Position).Length / explosiondistance)));
             }
         }
    }

    public void onDeleted()
    {
       
    }

    public EnemyManager EnemyManager { get; set; }
    public bool isActive { get; set; }
    public bool IsDead()
    {
        return isDead;
    }

    public void Draw(List<View> surface)
    {
        texture.setAtlasIndex(1, ligth? 1 : 2);
        texture.drawInfo.Position = Position;
        texture.drawInfo.Rotation = Rotation;
        surface[1].Draw(texture);
    }

    public bool applyDamage(int damage)
    {
        if (IsDead()) return false;
        explode();
        return true;
    }
}