using OpenTK.Windowing.Common;
using RadarGame.PhysicsSystem;

namespace RadarGame.Entities;

public static class EntityManager
{
    public static List<IEntitie> GameObjects { get; set; } = new List<IEntitie>();
    
    

    public static void Update(FrameEventArgs args)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Update(args);
        }
    }
    
    public static void AddObject(IEntitie gameObject)
    {
        GameObjects.Add(gameObject);
        if (gameObject is IPhysicsObject physicsObject)
        {
            PhysicsSystem.PhysicsSystem.AddObject(physicsObject);
        }

        if (gameObject is IDrawObject drawObject)
        {
            DrawSystem.DrawSystem.AddObject(drawObject);
        }
    }
    public static void RemoveObject(IEntitie gameObject)
    {
        GameObjects.Remove(gameObject);
        if (gameObject is IPhysicsObject physicsObject)
        {
            PhysicsSystem.PhysicsSystem.RemoveObject(physicsObject);
        }

        if (gameObject is IDrawObject drawObject)
        {
            DrawSystem.DrawSystem.RemoveObject(drawObject);
        }
    }
    
    public static void ClearObjects()
    {
        GameObjects.Clear();
        PhysicsSystem.PhysicsSystem.ClearObjects();
        DrawSystem.DrawSystem.ClearObjects();
    }
}