using App.Engine;

namespace RadarGame.DrawSystem;

public static class DrawSystem
{
    static List<IDrawObject> _drawObjects = new List<IDrawObject>();
    public static void Draw(View surface)
    {
        foreach (var drawObject in _drawObjects)
        {
            drawObject.Draw(surface);
        }
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