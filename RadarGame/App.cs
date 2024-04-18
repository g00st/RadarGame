using App.Engine;
using App.Engine.Template;
using Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace RadarGame;

public class App : EngineWindow
{
    ColoredRectangle test;   
    TexturedRectangle test2;
    
    public App() : base(1000, 1000, "Radargame")
    {
         test = new ColoredRectangle(new Vector2(0f, 0f), new Vector2(100f, 100f), Color4.Aqua);
         test2  =new TexturedRectangle(new Vector2(0f, 0f), new Vector2(100f, 100f), new Texture("resources/lol.jpg"));
        
        
        
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
        MainView.draw(test);
        MainView.draw(test2);
        
        this.SwapBuffers();

    }
    
    
    
}