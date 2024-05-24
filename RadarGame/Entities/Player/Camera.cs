using App.Engine;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame;
using RadarGame.Entities;
using RadarGame.Physics;

public class Camera : IEntitie, IDrawObject
{
    IEntitie target;
    private float zoom = 1;
    private float _rotation = 0;
    private Vector2 _position = new Vector2(0, 0);
    private Vector2 _baseSize = new Vector2(1920, 1080)* 0.5f;
    private Vector2 _size = new Vector2(1920, 1080) ;
    private float _maxZoom = 100;
    private float _minZoom = 10f;
    
  
    // Smoothing factors for position and rotation
    private float positionSmoothing = 0.05f;
    private float rotationSmoothing = 0.01f;

    public Camera(IEntitie target)
    {
        Name = "Camera";
        this.target = target;
        EntityManager.AddObject( new SarBackground());
    }

    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        // Calculate the difference between the camera's rotation and the target's rotation
        float rotationDifference = Math.Abs(WrapAngle(_rotation - ((IPhysicsObject)target).Rotation));

        // Adjust rotationSmoothing based on rotation difference
        rotationSmoothing = Math.Clamp(rotationDifference /  (float)Math.PI, 0.01f, 0.1f);

        _rotation = LerpAngle(_rotation, ((IPhysicsObject)target).Rotation, rotationSmoothing);
        _position = Vector2.Lerp(_position, ((IPhysicsObject)target).Position, positionSmoothing);

        zoom += zoom * 0.1f * mouseState.ScrollDelta.Y;
        zoom = Math.Clamp(zoom, _minZoom, _maxZoom);

        _size = _baseSize + new Vector2(200) * zoom;
    }

    private float LerpAngle(float a, float b, float t)
    {
        float delta = WrapAngle(b - a);

        // If delta > 180, subtract 360 to get the negative equivalent
        if (delta > Math.PI)
            delta -= 2 * (float)Math.PI;

        // If delta < -180, add 360 to get the positive equivalent
        if (delta < -Math.PI)
            delta += 2 * (float)Math.PI;

        return a + delta * t;
    }
    private float WrapAngle(float angle)
    {
        angle = (float)Math.IEEERemainder(angle, 2.0 * Math.PI);
        if (angle <= -Math.PI)
        {
            angle += 2.0f * (float)Math.PI;
        }
        else if (angle > Math.PI)
        {
            angle -= 2.0f * (float)Math.PI;
        }
        return angle;
    }
    public void onDeleted()
    {

    }

    public void Draw(List<View> surface)
    {
        surface[1].rotation = -_rotation;
        surface[1].vpossition = _position;
        surface[1].vsize = _size;
    }
}