using App.Engine;
using App.Engine.Template;
using Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RadarGame.Entities;
using RadarGame.DrawSystem;

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
    
    protected override void  OnUpdateFrame(FrameEventArgs args)
    {
        double time = args.Time;  
        base.OnUpdateFrame(args);
        EntityManager.Update( args);
        PhysicsSystem.PhysicsSystem.Update(time);
        
                        
    }
    
    protected override void Draw()
    {
        DrawSystem.DrawSystem.Draw(MainView);
    }
    
    protected override void Debugdraw()
    {
        PhysicsSystem.PhysicsSystem.DebugDraw();
    }
    
    
    
}