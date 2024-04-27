using App.Engine;
using App.Engine.Template;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace RadarGame.Radarsystem;

public  static class RadarSystem
{
    public static List<IRadarObject> RadarObjects { get; set; } = new List<IRadarObject>();
    private static TexturedRectangle testRectangle = new TexturedRectangle(new Vector2(0, 0), new Vector2(100, 100), new Texture("resources/cirno.png"));
   //-----------------AntennaStuff-----------------
    private static float AntanaRotation = 0;
    private static float Rotation = 0;
    private static bool sweep = false;
    private static float MaxAngle = 0;
    private static float MinAngle = 360;
    private  static Vector2 Position = new Vector2(0, 0);
    private static float  LastDistance = 0;
    private static Vector2 rotated = new Vector2(0, 0);
    
    private  enum RotationDir
    {
        Left,
        Right
    }
    private static RotationDir rotationDir = RotationDir.Right;
    private static float AntanaRotationSpeed = 0.0f;
    
    //-----------------RadarView-----------------
    private static Texture _Screentexture = new Texture(1000, 1000);
    private static  VBO screenVBO  = new VBO(_Screentexture);
    private static SubView RadarView = new SubView(screenVBO);
    private static RadarShader _radarShader = new RadarShader();
    private static TexturedRectangle radarRectangle = new TexturedRectangle(new Vector2(0, 0), new Vector2(1000, 1000), _Screentexture, _radarShader);
    public static void Update( FrameEventArgs args)
    {
        UpdateRotation();
        float direction = Rotation + AntanaRotation;
        rotated = new Vector2((float)Math.Sin(direction), (float)Math.Cos(direction));
        Vector2 newpoint = Raymarch(Position,rotated , 1000);
        LastDistance = (newpoint - Position).Length;
        
    }


    private static void UpdateRotation()
    {
        if (rotationDir == RotationDir.Right)
        {
            AntanaRotation += AntanaRotationSpeed;
        }
        else
        {
            AntanaRotation -= AntanaRotationSpeed;
        }

        if (sweep)
        {
            if (AntanaRotation > MaxAngle)
            {
                rotationDir = RotationDir.Left;
            }
            if (AntanaRotation < MinAngle)
            {
                rotationDir = RotationDir.Right;
            }
        }else
        {
            if (AntanaRotation > 360)
            {
                AntanaRotation = 0;
            }
            if (AntanaRotation < 0)
            {
                AntanaRotation = 360;
            }
        }


    }
    private static Vector2 Raymarch(Vector2 start, Vector2 direction, float maxDistance)
    {
        float distance = 0;
        for (int i = 0; i < 100; i++)
        {
            Vector2 position = start + direction * distance;
            float sdf = 1000000;
            foreach (var radarObject in RadarObjects)
            {
                sdf = Math.Min(sdf, radarObject.RadarSdf(position));
            }
            if (sdf < 0.1f)
            {
                return position;
            }
            distance += sdf;
            if (distance > maxDistance)
            {
                return start + direction * maxDistance;
            }
        }
        return start + direction * maxDistance;
    }
    
    public static void AddObject(IRadarObject radarObject)
    {
        RadarObjects.Add(radarObject);
    }
    public static void RemoveObject(IRadarObject radarObject)
    {
        RadarObjects.Remove(radarObject);
    }
    public static void clearObjects()
    {
        RadarObjects.Clear();
    }
    
    public static void render(){
        _radarShader.setAntennaRotation(Rotation);
        _radarShader.setDistance( LastDistance);
        _radarShader.setTextureSize(new Vector2(1000, 1000));
        RadarView._rendertarget.Bind();
        GL.ClearColor(Color4.Red);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        RadarView.Draw(radarRectangle);
        RadarView._rendertarget.Unbind();
    }
    public static void DebugDraw()
    {
       
       ImGuiNET.ImGui.Begin("Radar");
         ImGuiNET.ImGui.Text("AntennaRotation: " + AntanaRotation);
            ImGuiNET.ImGui.Text("Rotation: " + Rotation);
            ImGuiNET.ImGui.Text("Direction: " + rotated);
            ImGuiNET.ImGui.Text("Sweep: " + sweep);
            ImGuiNET.ImGui.Text("MaxAngle: " + MaxAngle);
            ImGuiNET.ImGui.Text("MinAngle: " + MinAngle);
            ImGuiNET.ImGui.Text("Position: " + Position);
            ImGuiNET.ImGui.Text("RotationDir: " + rotationDir);
            ImGuiNET.ImGui.Text("AntanaRotationSpeed: " + AntanaRotationSpeed);
            ImGuiNET.ImGui.Text("RadarObjects: " + RadarObjects.Count);
            ImGuiNET.ImGui.Text("LastDistance: " + LastDistance);
            ImGuiNET.ImGui.Image( _Screentexture.PtrHandle , new System.Numerics.Vector2(100,100));
           var d = ImGuiNET.ImGui.GetWindowDrawList() ;
           d.AddLine( new System.Numerics.Vector2(100,500), new System.Numerics.Vector2(100,500) + new System.Numerics.Vector2(rotated.X, rotated.Y) * 10, 0xFFFFFFFF);
           
       
    }
    
    public static void CleanUp()
    {
        RadarObjects.Clear();
        _Screentexture.Dispose();
        _radarShader.Dispose();
    }
    public static void SetPosition(Vector2 pos)
    {
        Position = pos;
    }
    public static void SetRotation(float rotation)
    {
        Rotation = rotation;
    }
    

}