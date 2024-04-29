using App.Engine;
using App.Engine.Template;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace RadarGame.Radarsystem;

public  static class RadarSystem
{
    public static List<System.Numerics.Vector3> Debugpoints { get; set; } = new List<System.Numerics.Vector3>();
    public static List<IRadarObject> RadarObjects { get; set; } = new List<IRadarObject>();
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
    private static double lastTime = 0;
    private static double delta = 0;
    private static float _RadarScreenrange = 10000;
    private static Texture _Screentexture = new Texture(1000, 1000);
    private static Texture _lastScreentexture = new Texture(1000, 1000);
    private static  VBO screenVBO  = new VBO(_Screentexture);
    private static  VBO lastScreenVBO  = new VBO(_lastScreentexture);
    private static SubView RadarView = new SubView(screenVBO);
    private static SubView LastRadarView = new SubView(lastScreenVBO);
    private static RadarShader _radarShader = new RadarShader();
    private static TexturedRectangle radarRectangle = new TexturedRectangle(new Vector2(0, 0), new Vector2(1000, 1000), _lastScreentexture, _radarShader);
    private static TexturedRectangle lastRadarRectangle = new TexturedRectangle(new Vector2(0, 0), new Vector2(1000, 1000),_Screentexture );
    
    
    public static void Update( FrameEventArgs args)
    {
        delta = args.Time - lastTime;
        lastTime = args.Time;
        UpdateRotation();
        float direction = Rotation + AntanaRotation + (float) (Math.PI /2.0f);
        rotated = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));
        Vector2 newpoint = Raymarch(Position,rotated , _RadarScreenrange);
        LastDistance = (newpoint - Position).Length;
        
    }


    private static void UpdateRotation()
    {  var lastRotation = AntanaRotation;
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
            if ( (lastRotation < MinAngle && AntanaRotation  >= MinAngle) ||
                 (lastRotation  > MaxAngle  && AntanaRotation <= MaxAngle)||
                 (lastRotation > MinAngle && AntanaRotation  <= MinAngle && false) ||
                 (lastRotation  < MaxAngle  && AntanaRotation >= MaxAngle && false)
                 )
            {
                rotationDir = (rotationDir == RotationDir.Right) ? RotationDir.Left : RotationDir.Right;
                AntanaRotation = lastRotation;
            }
        }
        AntanaRotation = (AntanaRotation + 2.0f * (float)Math.PI) % (2.0f *(float) Math.PI);
    }
    private static Vector2 Raymarch(Vector2 start, Vector2 direction, float maxDistance)
    {
        Debugpoints.Clear();
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
            Debugpoints.Add(new System.Numerics.Vector3(position.X, position.Y, sdf));
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
    
    public static void Render( ){
       
        
        
        _radarShader.setAntennaRotation(AntanaRotation + MathHelper.DegreesToRadians(180));
        _radarShader.setDistance( LastDistance);
        _radarShader.setTextureSize(new Vector2(1000, 1000));
        _radarShader.setRadarScreenrange(_RadarScreenrange);
        RadarView._rendertarget.Bind();
        GL.Clear(ClearBufferMask.ColorBufferBit);
        RadarView.Draw(radarRectangle);
        RadarView._rendertarget.Unbind();
        LastRadarView._rendertarget.Bind();
        GL.Clear(ClearBufferMask.ColorBufferBit);
        LastRadarView.Draw(lastRadarRectangle);
        LastRadarView._rendertarget.Unbind();
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
            ImGuiNET.ImGui.SliderFloat("RadarscrenRange" , ref _RadarScreenrange, 0, 10000);
            ImGuiNET.ImGui.SliderFloat("AntanaRotation" , ref AntanaRotation, 0, 2 * (float)Math.PI);
            ImGuiNET.ImGui.SliderFloat("rotationspeed" , ref AntanaRotationSpeed, 0, 0.1f);
            ImGuiNET.ImGui.Checkbox("Sweep", ref sweep);
            ImGuiNET.ImGui.SliderFloat("MaxAngle" , ref MaxAngle, 0, MathF.PI * 2);
            ImGuiNET.ImGui.SliderFloat("MinAngle" , ref MinAngle, 0, MathF.PI * 2);
            ImGuiNET.ImGui.Image( _Screentexture.PtrHandle , new System.Numerics.Vector2(500,500));
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