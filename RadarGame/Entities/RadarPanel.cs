using App.Engine;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class RadarPanel : IEntitie, IDrawObject
{
    //enety to display radar and radar controls
    
    public RadarPanel()
    {
        
    }

    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState)
    {
        throw new NotImplementedException();
    }

    public void onDeleted()
    {
        throw new NotImplementedException();
    }

    public void Draw(View surface)
    {
        throw new NotImplementedException();
    }
}