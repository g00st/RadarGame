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
   
  
    
    public App() : base(1000, 1000, "Radargame"){ 
        EntityManager.AddObject(new Background());
        EntityManager.AddObject(new PlayerObject( MainView.vpossition, 0f, "Player"));
        
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
        EntityManager.Update(args, KeyboardState);
        PhysicsSystem.Update(time);


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