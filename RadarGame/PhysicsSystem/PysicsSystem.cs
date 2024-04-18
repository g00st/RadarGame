namespace RadarGame.PhysicsSystem;

public  static class PhysicsSystem
{
    private static readonly List<IPhysicsObject> _physicsObjects = new List<IPhysicsObject>();
    public static void Update( double deltaTime)
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
    public static void AddObject(IPhysicsObject physicsObject)
    {
        _physicsObjects.Add(physicsObject);
    }
    public static void RemoveObject(IPhysicsObject physicsObject)
    {
        _physicsObjects.Remove(physicsObject);
    }
    
    public static void ClearObjects()
    {
        _physicsObjects.Clear();
    }
    
}