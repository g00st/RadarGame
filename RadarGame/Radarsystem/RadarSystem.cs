using App.Engine;
using App.Engine.Template;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Mathematics;

namespace RadarGame.Radarsystem;

public  static class RadarSystem
{
    public static List<System.Numerics.Vector3> Debugpoints { get; set; } = new List<System.Numerics.Vector3>();  //debugpoints for sdf <x,y radius>
    public static List<IRadarObject> RadarObjects { get; set; } = new List<IRadarObject>();
    //-----------------AntennaStuff-----------------
    private static float _antanaRotation = 0; //rotation of antenna 
    private static float _rotation = 0; //roation of "ship"
    private static bool _sweep = false; //sweeping radar
    private static float _maxAngle = 0 ; //max angle of radar swepp
    private static float _minAngle = 360; //min angle of radar swepp
    private  static Vector2 _position = new Vector2(0, 0); //position of radar in world
    private static float  _lastDistance = 0; //distance of last hit
    private static Vector2 _rotated = new Vector2(0, 0); //direction of radar but now as vector
    private static float _radarrange = 10000; //range of radar
    
    private  enum RotationDir
    {
        Left,
        Right
    }
    private static RotationDir _rotationDir = RotationDir.Right;
    private static float _antanaRotationSpeed = 0.02f;
    
    //-----------------RadarView-----------------
    private static double _lastTime = 0; 
    private static double _delta = 0; 
    private static float _radarScreenrange = 10000; //range of radar screen
    private static readonly Texture _Screentexture = new Texture(1000, 1000); //texture of radar screen 
    private static Texture _lastScreentexture = new Texture(1000, 1000); //texture of last radar screen for blending
    private static  VBO _screenVbo  = new VBO(_Screentexture); 
    private static  VBO _lastScreenVbo  = new VBO(_lastScreentexture); 
    private static SubView _lastRadarView = new SubView(_lastScreenVbo); 
    private static SubView _radarView = new SubView(_screenVbo); 
    private static RadarShader _radarShader = new RadarShader(); 
    private static TexturedRectangle _radarRectangle = new TexturedRectangle(new Vector2(0, 0), new Vector2(1000, 1000), _lastScreentexture, _radarShader);
    private static TexturedRectangle _lastRadarRectangle = new TexturedRectangle(new Vector2(0, 0), new Vector2(1000, 1000),_Screentexture );
    
    
    public static void Update( FrameEventArgs args)
    {
        _delta = args.Time - _lastTime;
        _lastTime = args.Time;
        UpdateRotation();
        float direction = _rotation + _antanaRotation + (float) (Math.PI /2.0f);
        _rotated = new Vector2((float)Math.Cos(direction), (float)Math.Sin(direction));
        Vector2 newpoint = Raymarch(_position,_rotated , _radarScreenrange);
        _lastDistance = (newpoint - _position).Length;
        
    }


    private static void UpdateRotation()
    {  var lastRotation = _antanaRotation;
        if (_rotationDir == RotationDir.Right)
        {
            _antanaRotation += _antanaRotationSpeed;
        }
        else
        {
            _antanaRotation -= _antanaRotationSpeed;
        }

        if (_sweep)
        {
            if ( (lastRotation < _minAngle && _antanaRotation  >= _minAngle) ||
                 (lastRotation  > _maxAngle  && _antanaRotation <= _maxAngle)||
                 (lastRotation > _minAngle && _antanaRotation  <= _minAngle && false) ||
                 (lastRotation  < _maxAngle  && _antanaRotation >= _maxAngle && false)
                 )
            {
                _rotationDir = (_rotationDir == RotationDir.Right) ? RotationDir.Left : RotationDir.Right;
                _antanaRotation = lastRotation;
            }
        }
        _antanaRotation = (_antanaRotation + 2.0f * (float)Math.PI) % (2.0f *(float) Math.PI);
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
    
    public static void setMaxAngle(float angle)
    {
        _maxAngle = angle;

    }
    
    public static float getMinAngle()
    {
        return  _minAngle;
    }
    public static float getMaxAngle()
    {
        return _maxAngle;
    }
    
    public static void setMinAngle(float angle)
    {
        _minAngle = angle;
    }
    
    public static void setSweep(bool s)
    {
        _sweep = s;
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

    public static Texture GetTexture()
    {
        return _Screentexture;
    }
    
    public static void Render( ){
       
        
        _radarShader.setRadarRange( _radarrange);
        _radarShader.setAntennaRotation(MathHelper.TwoPi- _antanaRotation );
        _radarShader.setDistance( _lastDistance);
        _radarShader.setTextureSize(new Vector2(1000, 1000));
        _radarShader.setRadarScreenrange(_radarScreenrange);
        _radarView._rendertarget.Bind();
        _radarView.Draw(_radarRectangle);
        _radarView._rendertarget.Unbind();
        _lastRadarView._rendertarget.Bind();
        _lastRadarView.Draw(_lastRadarRectangle);
        _lastRadarView._rendertarget.Unbind();
    }
    public static void DebugDraw()
    {
       
       ImGuiNET.ImGui.Begin("Radar");
         ImGuiNET.ImGui.Text("AntennaRotation: " + _antanaRotation);
            ImGuiNET.ImGui.Text("Rotation: " + _rotation);
            ImGuiNET.ImGui.Text("Direction: " + _rotated);
            ImGuiNET.ImGui.Text("Sweep: " + _sweep);
            ImGuiNET.ImGui.Text("MaxAngle: " + _maxAngle);
            ImGuiNET.ImGui.Text("MinAngle: " + _minAngle);
            ImGuiNET.ImGui.Text("Position: " + _position);
            ImGuiNET.ImGui.Text("RotationDir: " + _rotationDir);
            ImGuiNET.ImGui.Text("AntanaRotationSpeed: " + _antanaRotationSpeed);
            ImGuiNET.ImGui.Text("RadarObjects: " + RadarObjects.Count);
            ImGuiNET.ImGui.Text("LastDistance: " + _lastDistance);
            
            ImGuiNET.ImGui.SliderFloat("RadarscrenRange" , ref _radarScreenrange, 0, 10000);
            ImGuiNET.ImGui.SliderFloat("RadarRange" , ref _radarrange, 100, 10000);
            ImGuiNET.ImGui.SliderFloat("AntanaRotation" , ref _antanaRotation, 0, 2 * (float)Math.PI);
            ImGuiNET.ImGui.SliderFloat("rotationspeed" , ref _antanaRotationSpeed, 0, 0.1f);
            ImGuiNET.ImGui.Checkbox("Sweep", ref _sweep);
            ImGuiNET.ImGui.SliderFloat("MaxAngle" , ref _maxAngle, 0, MathF.PI * 2);
            ImGuiNET.ImGui.SliderFloat("MinAngle" , ref _minAngle, 0, MathF.PI * 2);
            ImGuiNET.ImGui.Image( _Screentexture.PtrHandle , new System.Numerics.Vector2(500,500));
           var d = ImGuiNET.ImGui.GetWindowDrawList() ;
           d.AddLine( new System.Numerics.Vector2(100,500), new System.Numerics.Vector2(100,500) + new System.Numerics.Vector2(_rotated.X, _rotated.Y) * 10, 0xFFFFFFFF);
    }
    
    public static void CleanUp()
    {
        RadarObjects.Clear();
        _Screentexture.Dispose();
        _radarShader.Dispose();
        _radarRectangle.Dispose();
        _lastScreentexture.Dispose();
        _lastRadarRectangle.Dispose();
    }
    public static void SetPosition(Vector2 pos)
    {
        _position = pos;
    }
    public static void SetRotation(float rotation)
    {
        _rotation = rotation;
    }
    

}