using System.Reflection.PortableExecutable;
using App.Engine;
using App.Engine.Template;
using ImGuiNET;
using OpenTK.Mathematics;
using RadarGame.Entities;

namespace RadarGame.DrawSystem;

public static class DrawSystem
{
   private static List<View> Layers = new List<View>();
   private static int count = 0;
   private static float _slowtime = 0;
   private static  Dictionary<String ,float> drawtimes = new Dictionary<String, float>();
   static  private View main ;

   static List<IDrawObject> _drawObjects = new List<IDrawObject>();
   static List<TexturedRectangle> _LayerTexturedRectangles = new List<TexturedRectangle>();
   private  static System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();
   private  static System.Diagnostics.Stopwatch _stopwatch2 = new System.Diagnostics.Stopwatch();
   
    private static float _fullTime = 0;
    private static float _clearTime = 0;
    private static float _drawTime = 0;
    private static float _drawLayertime = 0;

    public static void Init(View mainview, int layercount = 1)
    {
        main = mainview;
           for (int i = 0; i < layercount; i++)
            {
                Texture texture = new Texture((int)mainview.vsize.X , (int)mainview.vsize.Y);
                
                
                TexturedRectangle texturedRectangle = new TexturedRectangle(new Vector2(0, 0), mainview.vsize, texture, "Layer" + i);
                Layers.Add(new View(new VBO(texture)));
                _LayerTexturedRectangles.Add(texturedRectangle);
            }
    }
   
    public static void Draw(View surface)
    {
        _stopwatch.Restart();
        foreach (var layer in Layers)
        {
           layer.clear();
        }
        _clearTime = _stopwatch.ElapsedMilliseconds;
        _stopwatch.Restart();
        _stopwatch2.Restart();
        foreach (var drawObject in _drawObjects)
        {
           _stopwatch2.Restart();
            drawObject.Draw(Layers);
            drawtimes[((IEntitie) drawObject).Name ] = _stopwatch2.ElapsedTicks;
            
        }
        _drawTime = _stopwatch.ElapsedMilliseconds;
        _stopwatch.Restart();
        foreach (var rectangle in _LayerTexturedRectangles)
        {
            
            surface.Draw(rectangle);
            
        }
        _drawLayertime = _stopwatch.ElapsedMilliseconds;
        _fullTime = _drawTime + _clearTime + _drawLayertime;
        
    }
    
    
    public static View GetView( int layer =0 )
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
    
    public static void DebugDraw()
    {
        ImGuiNET.ImGui.Begin("DrawSystemDebug");
        ImGuiNET.ImGui.Text("Full Time: " + _fullTime);
        ImGuiNET.ImGui.Text("Slow Draw Time: " + _slowtime);
        ImGuiNET.ImGui.Text("Clear Time: " + _clearTime);
        ImGuiNET.ImGui.Text("Draw Time: " + _drawTime);
        ImGuiNET.ImGui.Text("Draw Layer Time: " + _drawLayertime);
        ImGuiNET.ImGui.Text("Slow Draw Count: " + count);
        
        //rest scrollable  drawtimes
        ImGuiNET.ImGui.Text("Draw Times:");
        ImGuiNET.ImGui.BeginChild("scrolling", new System.Numerics.Vector2(0, 0), ImGuiChildFlags.Border,
            ImGuiWindowFlags.HorizontalScrollbar);
        
        count = 0;
        _slowtime = 0;
        foreach (var drawtime in drawtimes)
        {
            if (drawtime.Value > 300)
            {
                count++;
                _slowtime +=   drawtime.Value/10000;
                ImGuiNET.ImGui.Text(drawtime.Key + " Time: " + drawtime.Value);
            }
        }
        
        ImGuiNET.ImGui.EndChild();
        
        ImGuiNET.ImGui.End();
    }
}