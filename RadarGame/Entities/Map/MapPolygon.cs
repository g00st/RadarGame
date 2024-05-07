using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;


namespace RadarGame.Entities;

public class MapPolygon : IEntitie , IDrawObject
{
    private Polygon _polygon;
    private ColoredRectangle _debugColoredRectangle;
    public Vector2 Position { get; set; }
    public Vector2 Rotation { get; set; }
    public List<Vector2> Points { get; set; }
    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
       return;
    }

    public void onDeleted()
    {
        return;
    }

    public MapPolygon(List<Vector2> points  , Vector2 position, Vector2 rotation, string name)
    {
        Points = points;
        Position = position;
        Rotation = rotation;
        Name = name;
        _polygon = new Polygon(points, new SimpleColorShader(Color4.Red), Position, new Vector2(1)  , 0, lines:true);
        _debugColoredRectangle = new ColoredRectangle(
            Position,
            new Vector2(10, 10),
            Color4.Aqua,
            Name,
            true
        );
    }
    
    public static MapPolygon CreateRandomPolygon(Vector2 center, Vector2 maxwidth, Vector2 minwidth, int pointCount, string name, Random random)
    {
       

        return new MapPolygon(points, center, new Vector2(0,0), name);
    }

    public void Draw(View surface)
    {
        
        surface.Draw(_polygon);
        surface.Draw(_debugColoredRectangle);
    }
    
     public static List<Vector2> GenerateRandomConvexPolygon(Vector2 bounds, int n, Random RAND)
    {
        // Generate two lists of random X and Y coordinates
        List<double> xPool = new List<double>(n);
        List<double> yPool = new List<double>(n);

        for (int i = 0; i < n; i++)
        {
            xPool.Add(RAND.NextDouble() * bounds.X);
            yPool.Add(RAND.NextDouble() * bounds.Y);
        }

        // Sort them
        xPool.Sort();
        yPool.Sort();

        // Isolate the extreme points
        double minX = xPool[0];
        double maxX = xPool[n - 1];
        double minY = yPool[0];
        double maxY = yPool[n - 1];

        // Divide the interior points into two chains & Extract the vector components
        List<double> xVec = new List<double>(n);
        List<double> yVec = new List<double>(n);

        double lastTop = minX, lastBot = minX;

        for (int i = 1; i < n - 1; i++)
        {
            double x = xPool[i];

            if (RAND.NextDouble() < 0.5)
            {
                xVec.Add(x - lastTop);
                lastTop = x;
            }
            else
            {
                xVec.Add(lastBot - x);
                lastBot = x;
            }
        }

        xVec.Add(maxX - lastTop);
        xVec.Add(lastBot - maxX);

        double lastLeft = minY, lastRight = minY;

        for (int i = 1; i < n - 1; i++)
        {
            double y = yPool[i];

            if (RAND.NextDouble() < 0.5)
            {
                yVec.Add(y - lastLeft);
                lastLeft = y;
            }
            else
            {
                yVec.Add(lastRight - y);
                lastRight = y;
            }
        }

        yVec.Add(maxY - lastLeft);
        yVec.Add(lastRight - maxY);

        // Randomly pair up the X- and Y-components
        Shuffle(yVec, RAND);

        // Combine the paired up components into vectors
        List<Vector2> vec = new List<Vector2>(n);

        for (int i = 0; i < n; i++)
        {
            vec.Add(new Vector2((float)xVec[i], (float)yVec[i]));
        }

        // Sort the vectors by angle
        vec.Sort((v1, v2) => Math.Atan2(v1.Y, v1.X).CompareTo(Math.Atan2(v2.Y, v2.X)));

        // Lay them end-to-end
        
        double x = 0, y = 0;
        double minPolygonX = 0;
        double minPolygonY = 0;
        List<Vector2> points = new List<Vector2>(n);

        for (int i = 0; i < n; i++)
        {
            points.Add(new Vector2((float)x, (float)y));

            x += vec[i].X;
            y += vec[i].Y;

            minPolygonX = Math.Min(minPolygonX, x);
            minPolygonY = Math.Min(minPolygonY, y);
        }

        // Move the polygon to the original min and max coordinates
        double xShift = minX - minPolygonX;
        double yShift = minY - minPolygonY;

        for (int i = 0; i < n; i++)
        {
            Vector2 p = points[i];
            points[i] = new Vector2(p.X + (float)xShift, p.Y + (float)yShift);
        }

        return points;
    }

    // Fisher-Yates shuffle algorithm for shuffling the Y components
    private static void Shuffle<T>(IList<T> list, Random RAND)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = RAND.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}