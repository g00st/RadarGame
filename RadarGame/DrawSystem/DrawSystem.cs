using System.Reflection.PortableExecutable;
using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;

namespace RadarGame.DrawSystem;

public static class DrawSystem
{
   private static List<SubView> Layers = new List<SubView>();
   static  private View main ;

   static List<IDrawObject> _drawObjects = new List<IDrawObject>();
   static List<TexturedRectangle> _LayerTexturedRectangles = new List<TexturedRectangle>();


    public static void Init(View mainview, int layercount = 1)
    {
        main = mainview;
           for (int i = 0; i < layercount; i++)
            {
                Texture texture = new Texture((int)mainview.vsize.X , (int)mainview.vsize.Y);
                
                
                TexturedRectangle texturedRectangle = new TexturedRectangle(new Vector2(0, 0), mainview.vsize, texture, "Layer" + i);
                Layers.Add(new SubView(new VBO(texture)));
                _LayerTexturedRectangles.Add(texturedRectangle);
            }
    }
   
    public static void Draw(View surface)
    {
        foreach (var layer in Layers)
        {
           layer.clear();
        }
        
        foreach (var drawObject in _drawObjects)
        {
            drawObject.Draw(Layers);
        }

        foreach (var rectangle in _LayerTexturedRectangles)
        {
            surface.Draw(rectangle);
            
        }
        
    }
    
    
    public static SubView GetView( int layer =0 )
    {
        return Layers[layer];
    }

    public static Vector2 ScreenToWorldcord(Vector2 screenpos , int layer =0)
    {
        var surface = Layers[layer];
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