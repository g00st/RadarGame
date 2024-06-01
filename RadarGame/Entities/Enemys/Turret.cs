using App.Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Physics;

namespace RadarGame.Entities.Enemys;

public class Turret : IEnemie, IDrawObject, IColisionObject, IcanBeHurt
{
    public PhysicsDataS PhysicsData { get; set; }
    public List<Vector2> CollisonShape { get; set; }
    public void OnColision(IColisionObject colidedObject)
    {
        throw new NotImplementedException();
    }
    

    public bool Static { get; set; }
    public Vector2 Position { get; set; }
    public Vector2 Center { get; set; }
    public float Rotation { get; set; }
    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        throw new NotImplementedException();
    }

    public void onDeleted()
    {
        throw new NotImplementedException();
    }

    public EnemyManager EnemyManager { get; set; }
    public bool IsDead()
    {
        throw new NotImplementedException();
    }

    public void Draw(List<View> surface)
    {
        throw new NotImplementedException();
    }

    public bool applyDamage(int damage)
    {
        throw new NotImplementedException();
    }
}