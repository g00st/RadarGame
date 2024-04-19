
using ImGuiNET;
using OpenTK.Mathematics;

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

    public static void DebugDraw()
    {
        ImGui.Begin("Physics Debug Window");
        ImGui.BeginChild("scrolling", new System.Numerics.Vector2(0, 0), ImGuiChildFlags.Border,
            ImGuiWindowFlags.HorizontalScrollbar);

        foreach (var physicsObject in _physicsObjects)
        {
            if (ImGui.CollapsingHeader(physicsObject.Name))
            {
                ImGui.PushID(physicsObject.GetHashCode());
                System.Numerics.Vector2 position =
                    new System.Numerics.Vector2(physicsObject.Position.X, physicsObject.Position.Y);
                ImGui.Text("Position X: " + physicsObject.Position.X + " Y: " + physicsObject.Position.Y);
                ImGui.Text("Velocity X: " + physicsObject.PhysicsData.Velocity.X + " Y: " +
                           physicsObject.PhysicsData.Velocity.Y);
                ImGui.Text("Acceleration X: " + physicsObject.PhysicsData.Acceleration.X + " Y: " +
                           physicsObject.PhysicsData.Acceleration.Y);
                ImGui.Text("Angular Velocity: " + physicsObject.PhysicsData.AngularVelocity);
                ImGui.Text("Angular Acceleration: " + physicsObject.PhysicsData.AngularAcceleration);
                ImGui.Text("Rotation: " + physicsObject.Rotation);
                ImGui.Text("Mass: " + physicsObject.PhysicsData.Mass);
                ImGui.Text("Drag: " + physicsObject.PhysicsData.Drag);
                ImGui.Text("Center X: " + physicsObject.Center.X + " Y: " + physicsObject.Center.Y);
                ImGui.PopID();
             
              
            }  
            
        }
        ImGui.EndChild();
        ImGui.End();
        
        
    }

}