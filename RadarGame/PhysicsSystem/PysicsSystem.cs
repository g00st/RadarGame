namespace RadarGame.PhysicsSystem;

public class PhysicsSystem
{
    private readonly List<IPhysicsObject> _physicsObjects = new List<IPhysicsObject>();
    public void Update( double deltaTime)
    {
        foreach (var physicsObject in _physicsObjects)
        {
            physicsObject.PhysicsData = physicsObject.PhysicsData with
            {
                AngularVelocity = physicsObject.PhysicsData.AngularAcceleration * (float)deltaTime + physicsObject.PhysicsData.AngularVelocity,
                Velocity = physicsObject.PhysicsData.Velocity * physicsObject.PhysicsData.Drag, 
            };
        }
        Console.WriteLine("PhysicsSystem Update");
    }
    public void AddObject(IPhysicsObject physicsObject)
    {
        _physicsObjects.Add(physicsObject);
    }
    public void RemoveObject(IPhysicsObject physicsObject)
    {
        _physicsObjects.Remove(physicsObject);
    }
    
    public void ClearObjects()
    {
        _physicsObjects.Clear();
    }
    
}