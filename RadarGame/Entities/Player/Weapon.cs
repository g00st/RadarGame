using App.Engine;
using App.Engine.Template;
using OpenTK.Windowing.Common;

namespace RadarGame.Entities;

public class Weapon
{
    public int Damage { get; set; }
    public int cooldown { get; set; }
    public int energyCost { get; set; }
    public string Name { get; set; }
    public TexturedRectangle icon { get; set; }
    public string Description { get; set; }
    public Weponstate state { get; set; }
    
    public enum Weponstate
    {
     ready,
     fiering,
     coolingdown
    }
    
    public virtual void fire()
    {
        
    }
    
    public virtual void Update( FrameEventArgs args )
    {
        
    }
    
}