using App.Engine;
using App.Engine.Template;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class Background : IEntitie , IDrawObject
{
    public string Name { get; set; }
    private TexturedRectangle _background;
    
    public Background()
    {
        Name = "Background";
        _background = new TexturedRectangle(new Texture("resources/background.jpg"));
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState)
    {
       
    }

    public void onDeleted()
    {
        _background.Dispose();
    }

    public void Draw(View surface)
    {
      surface.Draw(_background);
    }
}