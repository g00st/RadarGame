using OpenTK.Windowing.Common;
using RadarGame.Physics;

namespace RadarGame.Entities;

public static class EntityManager
{
    public static List<IEntitie> GameObjects { get; set; } = new List<IEntitie>();
    public static List<String> Names { get; set; } = new List<string>();



    public static void Update(FrameEventArgs args)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Update(args);
        }
    }
    
    public static void AddObject(IEntitie gameObject)
    {
        if (Names.Contains(gameObject.Name))
        {
            throw new Exception("Name already exists");
        }
        Names.Add(gameObject.Name);
        GameObjects.Add(gameObject);
        if (gameObject is IPhysicsObject physicsObject)
        {
            Physics.PhysicsSystem.AddObject(physicsObject);
        }

        if (gameObject is IDrawObject drawObject)
        {
            DrawSystem.DrawSystem.AddObject(drawObject);
        }
    }
    public static void RemoveObject(IEntitie gameObject)
    {
        Names.Remove(gameObject.Name);
        GameObjects.Remove(gameObject);
        if (gameObject is IPhysicsObject physicsObject)
        {
            Physics.PhysicsSystem.RemoveObject(physicsObject);
        }

        if (gameObject is IDrawObject drawObject)
        {
            DrawSystem.DrawSystem.RemoveObject(drawObject);
        }
    }
    public static IEntitie GetObject(string name)
    {
        return GameObjects.Find(x => x.Name == name);
    }
    public static List<T> GetObjectsbyType<T>( T type)
    {
        return GameObjects.FindAll(x => x is T).Cast<T>().ToList();
    }
   


    public static void ClearObjects()
    {
        GameObjects.Clear();
        Physics.PhysicsSystem.ClearObjects();
        DrawSystem.DrawSystem.ClearObjects();
    }
}