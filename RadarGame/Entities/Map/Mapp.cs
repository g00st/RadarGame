using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Physics;


namespace RadarGame.Entities;

public class Mapp : IEntitie
{
    private Vector2 MapSize; //size of map
    private Vector2 MapPosition; //center of map
    private List<MapPolygon > MapPolygons; //list of polygons that make up the map


    public string Name { get; set; }
    
    
    
public Mapp(Vector2 mapSize, Vector2 mapPosition)
    {
        MapSize = mapSize;
        MapPosition = mapPosition;
        Name = "Mapp";
        MapPolygons = new List<MapPolygon>();
        CreateBorder();
        
        //add some random polygons to the map
        Random random = new Random(); 
        for (int i = 0; i < 10; i++)
        {
            Vector2 center = new Vector2((float)random.NextDouble() * MapSize.X - MapSize.X / 2, (float)random.NextDouble() * MapSize.Y - MapSize.Y / 2);
            Vector2 maxwidth = new Vector2((float)random.NextDouble() * 100 + 50, (float)random.NextDouble() * 100 + 50);
            Vector2 minwidth = new Vector2((float)random.NextDouble() * 50, (float)random.NextDouble() * 50);
            int pointCount = random.Next(3, 10);
            MapPolygon c = MapPolygon.CreateRandomPolygon( center, maxwidth, minwidth, pointCount, "RandomPolygon" + i, random);
            MapPolygons.Add( c);
        }
        
        foreach( var polygon in MapPolygons)
        {
            EntityManager.AddObject(polygon);
        }
        
    }

    private void CreateBorder()
    {
       // add 4 polygons to the map that make up the border
       MapPolygon top = new MapPolygon(
           new List<Vector2>()
           {
               new Vector2(-MapSize.X/2,MapSize.Y / 2),
               new Vector2(MapSize.X/2,MapSize.Y / 2),
               new Vector2(MapSize.X/2,MapSize.Y  / 2+ 100),
               new Vector2(-MapSize.X/2,MapSize.Y / 2 + 100)
           }, new Vector2(0,0), 
           new Vector2(0,0), "Top");
       
         MapPolygon bottom = new MapPolygon(
              new List<Vector2>()
              {
                new Vector2(-MapSize.X/2,-MapSize.Y / 2),
                new Vector2(MapSize.X/2,-MapSize.Y  / 2),
                new Vector2(MapSize.X/2,-MapSize.Y  / 2- 100),
                new Vector2(-MapSize.X/2,-MapSize.Y / 2 - 100)
              }, new Vector2(0,0), 
              new Vector2(0,0), "Bottom");
         
            MapPolygon left = new MapPolygon(
                new List<Vector2>()
                {
                    new Vector2(-MapSize.X  / 2,-MapSize.Y/2),
                    new Vector2(-MapSize.X  / 2,MapSize.Y/2),
                    new Vector2(-MapSize.X / 2 - 100,MapSize.Y/2),
                    new Vector2(-MapSize.X  / 2- 100,-MapSize.Y/2)
                }, new Vector2(0,0), 
                new Vector2(0,0), "Left");
            
            MapPolygon right = new MapPolygon(
                
    new List<Vector2>()
                {
                    new Vector2(MapSize.X / 2,-MapSize.Y/2),
                    new Vector2(MapSize.X / 2,MapSize.Y/2),
                    new Vector2(MapSize.X  / 2+ 100,MapSize.Y/2),
                    new Vector2(MapSize.X  / 2+ 100,-MapSize.Y/2)
                }, new Vector2(0,0), 
                new Vector2(0,0), "Right");
            
            
            MapPolygons.Add(top);
            MapPolygons.Add(bottom);
            MapPolygons.Add(left);
            MapPolygons.Add(right);
    }
    
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
      
    }

    public void onDeleted()
    {
        
    }
}