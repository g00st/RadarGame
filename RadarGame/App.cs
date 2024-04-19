using App.Engine;
using App.Engine.Template;
using Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using RadarGame.Entities;
using RadarGame.PhysicsSystem;
using RadarGame.DrawSystem;

namespace RadarGame;

public class App : EngineWindow
{
    ColoredRectangle test;   
    TexturedRectangle test2;
    List<IDrawObject> scene = new List<IDrawObject>();
    
    public App() : base(1000, 1000, "Radargame")
    {
         test = new ColoredRectangle(new Vector2(0f, 0f), new Vector2(100f, 100f), Color4.Aqua);
         test2  =new TexturedRectangle(new Vector2(0f, 0f), new Vector2(100f, 100f), new Texture("resources/lol.jpg"));
         GameObject gameObject = new GameObject();
         EntityManager.AddObject(gameObject);
        
        
    }
    
    protected override void  OnUpdateFrame(FrameEventArgs args)
    {
        double time = args.Time;  
        base.OnUpdateFrame(args);
        
        PhysicsSystem.PhysicsSystem.Update(time);
        
                        
    }
    
    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        DrawSystem.DrawSystem.Draw(MainView);
        
        this.SwapBuffers();

    }
    
    
    
}