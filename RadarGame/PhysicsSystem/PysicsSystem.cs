namespace RadarGame.PhysicsSystem;

public  static class PhysicsSystem
{
    private static readonly List<IPhysicsObject> _physicsObjects = new List<IPhysicsObject>();
    public static void Update( double deltaTime)
    {
        foreach (var physicsObject in _physicsObjects)
        {
            
            var newVel = physicsObject.PhysicsData.Velocity  +physicsObject.PhysicsData.Acceleration * (float)deltaTime;
            newVel = newVel * (1 - physicsObject.PhysicsData.Drag);
            var newAngVel = physicsObject.PhysicsData.AngularVelocity + physicsObject.PhysicsData.AngularAcceleration * (float)deltaTime;
            
            physicsObject.PhysicsData = physicsObject.PhysicsData with {Velocity = newVel, AngularVelocity = newAngVel};
            physicsObject.Position += physicsObject.PhysicsData.Velocity * (float)deltaTime;
            physicsObject.Rotation += physicsObject.PhysicsData.AngularVelocity * (float)deltaTime;
            
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