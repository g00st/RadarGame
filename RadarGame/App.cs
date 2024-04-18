using App.Engine.Template;
using Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace RadarGame;

public class App : EngineWindow
{
    
    
    public App() : base(1000, 1000, "Radargame")
    {
        ColoredRectangle test = new ColoredRectangle(new Vector2(0f, 0f), new Vector2(100f, 100f), Color4.Aqua);
        MainView.addObject(test);
        
        
    }
    
    protected override void  OnUpdateFrame(FrameEventArgs args)
    {
        double time = args.Time;  
        base.OnUpdateFrame(args);
        
        PhysicsSystem.Update(time);
        
        
       
                        
    }
    
    
    
}