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
        _background = new TexturedRectangle(new Texture("resources/background2.jpg"));
        _background.drawInfo.Size = new OpenTK.Mathematics.Vector2(1920, 1280);
        _background.drawInfo.Position = new OpenTK.Mathematics.Vector2(0,-100);
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
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