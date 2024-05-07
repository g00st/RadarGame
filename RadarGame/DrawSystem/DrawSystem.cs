using App.Engine;
using OpenTK.Mathematics;

namespace RadarGame.DrawSystem;

public static class DrawSystem
{
   static  private View main ;

   static List<IDrawObject> _drawObjects = new List<IDrawObject>();
    public static void Draw(View surface)
    {
        main = surface;
        foreach (var drawObject in _drawObjects)
        {
            drawObject.Draw(surface);
        }
    }
    
    
    public static View GetMainView()
    {
        return main;
    }

    public static Vector2 ScreenToWorldcord(Vector2 screenpos , View surface)
    {
        
        return new Vector2(screenpos.X / surface.Width * surface.vsize.X , screenpos.Y / surface.Height * surface.vsize.Y);
    }
   
    
    public static void AddObject(IDrawObject drawObject)
    {
        _drawObjects.Add(drawObject);
    }
    
    public static void RemoveObject(IDrawObject drawObject)
    {
        _drawObjects.Remove(drawObject);
    }
    
    public static void ClearObjects()
    {
        _drawObjects.Clear();
    }
}