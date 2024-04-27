using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace App.Engine.Template;

public class Polygon : DrawObject , IDisposable
{
    /// <summary>
    /// Takes a list of normalized points and creates a polygon from them
    /// </summary>
    public DrawInfo drawInfo { get; }
    public Vector2 Position { get => drawInfo.Position; set => drawInfo.Position = value; }
    public Vector2 Size { get => drawInfo.Size; set => drawInfo.Size = value; }
    public float Rotation { get => drawInfo.Rotation; set => drawInfo.Rotation = value; }

    public  Polygon(List<Vector2> Points, Shader shader,Vector2 position, Vector2 size ,float rotation , string name = "Polygon", bool lines = false)
    {
        if (lines)
        {
            this.drawInfo = new DrawInfo(position, size, rotation, CreateLineMesh(Points, lines), name);
            
        }
        else
        {
            this.drawInfo = new DrawInfo(position, size, rotation, CreateLineMesh(Points, lines), name);
        }
        this.drawInfo.mesh.Shader = shader;


    }
   

    public static Polygon Circle(Vector2 Position, float Radius, int Segments, Shader shader , string name = "Circle", bool lines = true)
    {
        List<Vector2> points = new List<Vector2>();
        for (int i = 0; i < Segments; i++)
        {
            float angle = MathHelper.DegreesToRadians(360.0f / Segments * i);
            points.Add(new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)));
        }

        foreach (var point in points)
        {
            Console.WriteLine(point);
        }
        Console.WriteLine("Circle Created: "+ points.Count + " points");

        return new Polygon(points,shader, Position, new Vector2(Radius, Radius),0, name);
    }
    public static Polygon Rectangle(Vector2 Position, Vector2 Size,float rotation , Shader shader, string name = "Rectangle", bool center = false, bool lines = true)
    {
        List<Vector2> points = new List<Vector2>();
           if (center)
            {
                points.Add(new Vector2(-0.5f, -0.5f));
                points.Add(new Vector2(0.5f, -0.5f));
                points.Add(new Vector2(0.5f, 0.5f));
                points.Add(new Vector2(-0.5f, 0.5f));
            }
            else
            {
                points.Add(new Vector2(0.0f, 0.0f));
                points.Add(new Vector2(1.0f, 0.0f));
                points.Add(new Vector2(1.0f, 1.0f));
                points.Add(new Vector2(0.0f, 1.0f));
            }
           return new Polygon(points, shader, Position, Size, rotation,name, lines);
    }
    public static Polygon Line (Vector2 start, Vector2 end, Shader shader, string name = "Line")
    {
        List<Vector2> points = new List<Vector2>();
        points.Add(start);
        points.Add(end);
        return new Polygon(points, shader, new Vector2(0,0), new Vector2(1,1), 0, name, true);
    }
   
    
    private Mesh CreateLineMesh(List<Vector2> Points, bool lines)
    {
        Mesh mesh = new Mesh();
        Bufferlayout bufferlayout = new Bufferlayout();
        bufferlayout.count = 2;
        bufferlayout.normalized = false;
        bufferlayout.offset = 0;
        bufferlayout.type = VertexAttribType.Float;
        bufferlayout.typesize = sizeof(float);
        //create lines from points 
        float[] vertices = Points.SelectMany(v => new float[] { v.X, v.Y }).ToArray();
        float [] indecies = new float[Points.Count];
        for (int i = 0; i < indecies.Length; i++)
        {
            indecies[i] = i;
        }
        
        
        mesh.AddAtribute(bufferlayout, vertices);
        mesh.AddIndecies(indecies.Select(i => (uint)i).ToArray());
        mesh.PrimitiveType = PrimitiveType.LineLoop;
        return mesh;
    }
    
    



    public void Dispose()
    {
        drawInfo.Dispose();
    }
}

