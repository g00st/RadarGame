using System.Drawing;
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
    private List<Vector4> Ocuppied = new List<Vector4>(); //list of ocuppied areas

    public string Name { get; set; }
    private int Count = 0;
    
    
    
public Mapp(Vector2 mapSize, Vector2 mapPosition)
    {
        MapSize = mapSize;
        MapPosition = mapPosition;
        Name = "Mapp";
        MapPolygons = new List<MapPolygon>();
        CreateBorder();
        
        List<Vector4> borders = new List<Vector4>(this.Ocuppied);
        
        //add some random polygons to the map
        Random random = new Random(); 
        RectanglePack(100, 5000, 10, 200, 1000);
        
        
        
        foreach (var rect  in Ocuppied)
        {
            var x= MapPolygon.CreateRandomPolygon( new Vector2(rect.X + (rect.Z - rect.X) / 2, rect.Y + (rect.W - rect.Y) / 2), new Vector2(rect.Z - rect.X, rect.W - rect.Y), 100, 10, "RandomPolygon" + Ocuppied.IndexOf(rect), random);
            MapPolygons.Add(x);
        }
        
        foreach( var polygon in MapPolygons)
        {
            EntityManager.AddObject(polygon);
        }
        
    }

    private void RectanglePack(float initialSize, float maxSize, float sizeIncrement, int initialCount, float spacing)
    {
        Random random = new Random();

        // Generate a random number of small rectangles
        for (int i = 0; i < initialCount; i++)
        {
            float x = MapPosition.X - MapSize.X / 2 + spacing + (float)random.NextDouble() * (MapSize.X - initialSize - 2 * spacing);
            float y = MapPosition.Y - MapSize.Y / 2 + spacing + (float)random.NextDouble() * (MapSize.Y - initialSize - 2 * spacing);
            Vector4 newRect = new Vector4(x, y, x + initialSize, y + initialSize);

            if (!CheckCollision(newRect, new Vector4()) && IsWithinMapBounds(newRect) && !IsInStartingArea(newRect))
            {
                Ocuppied.Add(newRect);
            }
        }

        // Start with small rectangles
        float size = initialSize;

        // While there's space left in the map
        while (size <= maxSize)
        {
            bool sizeIncreased = false;

            // For each rectangle in the Ocuppied list
            for (int i = 0; i < Ocuppied.Count; i++)
            {
                Vector4 rect = Ocuppied[i];

                // Increase the size of the rectangle
                Vector4 newRect = new Vector4(rect.X, rect.Y, rect.Z + sizeIncrement, rect.W + sizeIncrement);

                // If the new size causes a collision with any other rectangle or it goes out of the map bounds, revert the size increase
                if (CheckCollision(newRect, rect) || !IsWithinMapBounds(newRect) || IsInStartingArea(newRect))
                {
                    continue;
                }

                // Otherwise, update the rectangle in the Ocuppied list
                Ocuppied[i] = newRect;
                sizeIncreased = true;
            }

            // If no rectangles could be increased in size, break the loop
            if (!sizeIncreased)
            {
                break;
            }

            // Increase the size for the next iteration
            size += sizeIncrement;
        }
    }

private bool IsWithinMapBounds(Vector4 rect)
{
    return rect.X >= MapPosition.X - MapSize.X / 2 && rect.Y >= MapPosition.Y - MapSize.Y / 2 && rect.Z <= MapPosition.X + MapSize.X / 2 && rect.W <= MapPosition.Y + MapSize.Y / 2;
}

private bool IsInStartingArea(Vector4 rect)
{
    float startingAreaSize = 2000;
    float halfSize = startingAreaSize / 2;

    // Check if any part of the rectangle is within the starting area
    if (rect.Z > -halfSize && rect.X < halfSize && rect.W > -halfSize && rect.Y < halfSize)
    {
        // The rectangle is within the starting area
        return true;
    }

    // The rectangle is not within the starting area
    return false;
}




    

    private void CreateBorder()
    {
       float borderSize = 100;
       // add 4 polygons to the map that make up the border
            List<Vector2> points = new List<Vector2>();
            points.Add(new Vector2(0.5f, 0.5f));
            points.Add(new Vector2(-0.5f, 0.5f));
            points.Add(new Vector2(-0.5f, -0.5f));
            points.Add(new Vector2(0.5f, -0.5f));
            
            MapPolygon top = new MapPolygon(points, new Vector2(0, MapSize.Y / 2 + borderSize /2), new Vector2(0, 0), new Vector2(MapSize.X, borderSize), "Top");
            MapPolygon bottom = new MapPolygon(points, new Vector2(0, -MapSize.Y / 2- borderSize /2), new Vector2(0, 0), new Vector2(MapSize.X, borderSize), "Bottom");
            MapPolygon left = new MapPolygon(points, new Vector2(-MapSize.X / 2- borderSize /2, 0), new Vector2(0, 0), new Vector2(borderSize, MapSize.Y), "Left");
            MapPolygon right = new MapPolygon(points, new Vector2(MapSize.X / 2+ borderSize /2, 0), new Vector2(0, 0), new Vector2(borderSize, MapSize.Y), "Right");
            
            //adthe for rectangles as occupied
            borderSize /= 2;
            
            Vector4 topRect = new Vector4(-MapSize.X / 2, MapSize.Y / 2, MapSize.X / 2, MapSize.Y / 2 + borderSize);
            Vector4 bottomRect = new Vector4(-MapSize.X / 2, -MapSize.Y / 2 - borderSize, MapSize.X / 2, -MapSize.Y / 2);
            Vector4 leftRect = new Vector4(-MapSize.X / 2 - borderSize, -MapSize.Y / 2, -MapSize.X / 2, MapSize.Y / 2);
            Vector4 rightRect = new Vector4(MapSize.X / 2, -MapSize.Y / 2, MapSize.X / 2 + borderSize, MapSize.Y / 2);
            
            
            
            MapPolygons.Add(top);
            MapPolygons.Add(bottom);
            MapPolygons.Add(left);
            MapPolygons.Add(right);
    }
    
    public bool CheckCollision(Vector4 newRect, Vector4 ignoreRect)
    {
        foreach (var occupiedRect in Ocuppied)
        {
            if (occupiedRect == ignoreRect)
            {
                continue;
            }
            
            if (IsColliding(newRect, occupiedRect))
            {
                return true; 
            }
        }
        return false;
    }
    private bool IsColliding(Vector4 rect1, Vector4 rect2)
    {
        return (rect1.X < rect2.Z && rect1.Z > rect2.X &&
                rect1.Y < rect2.W && rect1.W > rect2.Y);
    }
    
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
       if( Count < 500)
       {
           
           Random random = new Random();
           Vector2 p = new Vector2( (float)random.NextDouble() * 5000 -2500, (float)random.NextDouble()  * 5000 -2500);
           float distance = 0;
           ColisionSystem.getNearest(p, out distance);
              if (distance < 100)
              {
                return;
              }
           EntityManager.AddObject(new GameObject(p, 0, "RandomObject" + Count, new Vector2((float)random.NextDouble() * 100 - 50, (float)random.NextDouble() * 100 - 50), (float)random.NextDouble() * 10 - 5));
          Count++; 
       }
      
    }

    public void onDeleted()
    {
        
    }
}