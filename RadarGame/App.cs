using App.Engine;
using App.Engine.Template;
using Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Entities;
using RadarGame.DrawSystem;
using RadarGame.Physics;

namespace RadarGame;

public class App : EngineWindow
{
    ColoredRectangle test;   
    TexturedRectangle test2;
    List<IDrawObject> scene = new List<IDrawObject>();
    
    public App() : base(1000, 1000, "Radargame")
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject gameObject = new GameObject( MainView.vpossition, 0f, "test"+i);
            EntityManager.AddObject(gameObject);
        }
       
        
        
        
    }


    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        double time = args.Time;
        base.OnUpdateFrame(args);
        EntityManager.Update(args);
        PhysicsSystem.Update(time);
        
        
        // TODO: inputsystem needed oder KeyboardState im update immer an alle entetys mitgeben 
        if (KeyboardState.IsKeyDown(Keys.W))
        {
            PhysicsSystem.ApplyForce((IPhysicsObject)EntityManager.GetObject("test0"),
                
                new Vector2(0f, 100f));
        }
        if (KeyboardState.IsKeyDown(Keys.A))
        {
            PhysicsSystem.ApplyForce((IPhysicsObject)EntityManager.GetObject("test0"),
                new Vector2(-100f, 0f));
        }
        if (KeyboardState.IsKeyDown(Keys.S))
        {
            PhysicsSystem.ApplyForce((IPhysicsObject)EntityManager.GetObject("test0"),
                new Vector2(0f, -100f));
        }
        if (KeyboardState.IsKeyDown(Keys.D))
        {
            PhysicsSystem.ApplyForce((IPhysicsObject)EntityManager.GetObject("test0"),
                new Vector2(100f, 0f));
        }
        Console.WriteLine(((IPhysicsObject)EntityManager.GetObject("test0")).PhysicsData.Velocity);
    }

    protected override void Draw()
    {
        DrawSystem.DrawSystem.Draw(MainView);
    }
    
    protected override void Debugdraw()
    {
        Physics.PhysicsSystem.DebugDraw();
    }
    
    
    
}