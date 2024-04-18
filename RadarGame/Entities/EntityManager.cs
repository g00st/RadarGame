using RadarGame.PhysicsSystem;

namespace RadarGame.Entities;

public class EntityManager
{
    public List<IEntitie> GameObjects { get; set; }
    
    public EntityManager()
    {
        GameObjects = new List<IEntitie>();
    }

    public void Update(double deltaTime)
    {
        foreach (var gameObject in GameObjects)
        {
            gameObject.Update(deltaTime);
        }
    }
    
    public void AddObject(IEntitie gameObject)
    {
        GameObjects.Add(gameObject);
        if (gameObject is IPhysicsObject physicsObject)
        {
            PhysicsSystem.PhysicsSystem.AddObject(physicsObject);
        }

       
        
    }
}