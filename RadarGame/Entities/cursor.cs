using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class cursor: IEntitie , IDrawObject
{
    public string Name { get; set; }
    private ColoredRectangle _cursor;
    
    public cursor( string name = "cursor"
    ){
        Name = name;
        _cursor = new ColoredRectangle(new OpenTK.Mathematics.Vector2(0, 0), new OpenTK.Mathematics.Vector2(10, 10), Color4.Aqua, "cursor", true);
    }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        var x = new OpenTK.Mathematics.Vector2(mouseState.X, mouseState.Y);
        x = DrawSystem.DrawSystem.ScreenToWorldcord(x);
       _cursor.drawInfo.Position = x;
    }

    public void onDeleted()
    {
        _cursor.Dispose();
    }

  

    public void Draw(List<View> surface)
    {
        surface[0].Draw(_cursor);
    }
}
