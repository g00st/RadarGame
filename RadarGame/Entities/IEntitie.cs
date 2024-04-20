using OpenTK.Windowing.Common;

namespace RadarGame.Entities;

public interface IEntitie
{ 
    public String Name { get; set; }
    public void Update(FrameEventArgs args);
}