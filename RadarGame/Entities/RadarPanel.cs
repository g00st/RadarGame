using System.Drawing;
using OpenTK.Mathematics;
using App.Engine;
using App.Engine.Template;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Radarsystem;

namespace RadarGame.Entities;

public class RadarPanel : IEntitie, IDrawObject
{
    //enety to display radar and radar controls
    
    private Vector2 Position { get; set; }
    private Vector2 Center { get; set; }
    private Vector2 Size { get;  set; }
    private TexturedRectangle _display; //display of radar
    private CircularSlider _cirlceslider1;
    private CircularSlider _cirlceslider2;
    private Button _SweepButton; //button to turn on and off radar sweep
    private TexturedRectangle _background ; //slider for radar screen range
    private TexturedRectangle _rim; //rim of radar
    private TexturedRectangle _ButtonLabel;
    private float maxangle = 0;
    private float minangle = 2 * (float)Math.PI;
    
    public RadarPanel()
    {
        Position = new Vector2(0, 0);
        Size = new Vector2(800, 800);
        Center = new Vector2(Size.X / 2, Size.Y / 2) + Position + new Vector2(0, Size.Y / 30);
        Texture texture = new Texture("resources/Radar/slider_circular.png");
        
        _display = new TexturedRectangle(Center, Size- Size /5 , RadarSystem.GetTexture(),"RadarPanel_display",true);
        _background = new TexturedRectangle(Position, Size, new Texture( "resources/Radar/bg.jpg"));
        _rim = new TexturedRectangle(Center, Size - Size/10, new Texture("resources/Radar/Radarrim.png"), "RadarPanel_rim", true);
        _cirlceslider1 = new CircularSlider(Center, _display.drawInfo.Size.X/2 , 0,texture, new Vector2(Size.X /20,Size.Y /10));
        _cirlceslider2 = new CircularSlider(Center, _display.drawInfo.Size.X/2, 1,texture,  new Vector2(Size.X /20,Size.X /10));
        _cirlceslider1.sidergroup = new List<CircularSlider>(){_cirlceslider2};
        _cirlceslider2.sidergroup = new List<CircularSlider>(){_cirlceslider1};
        _ButtonLabel = new TexturedRectangle(Position + Size - new Vector2(Size.X/8, Size.Y /16), new Vector2(Size.X /10.6f, Size.Y /32), new Texture("resources/Radar/Sweep.png"), "label",true);
        _SweepButton = new Button(Position +Size - new Vector2(Size.X/6.4f), new Vector2(Size.X /16), new Texture("resources/Radar/Button_on.png"), new Texture("resources/Radar/Button_of.png"), new Texture("resources/Radar/Button_on_hover.png"), new Texture("resources/Radar/Button_of_hover.png"));

    }

    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
       
        //  throw new NotImplementedException();
        _cirlceslider1.Update(mouseState);
        _cirlceslider2.Update(mouseState);
        _SweepButton.Update(mouseState , args);
      
        minangle = _cirlceslider1.angle - (float)Math.PI/2;
        maxangle = _cirlceslider2.angle - (float)Math.PI/2;
        
         minangle = (float)((minangle + 2 * Math.PI  ) % (2 * Math.PI));
         maxangle = (float)((maxangle + 2 * Math.PI) % (2 * Math.PI));
        
        RadarSystem.setMinAngle( minangle) ;
        RadarSystem.setMaxAngle( maxangle);
        RadarSystem.setSweep(_SweepButton.isOn());
        
    }

    public void onDeleted()
    {
        ///throw new NotImplementedException();
        
        _background.Dispose();
        
        _display.Dispose();
        _rim.Dispose();
        _cirlceslider1.slider.Dispose();
        _cirlceslider2.slider.Dispose();
        _SweepButton.Dispose();
    }

    public void Draw(View surface)
    {
        surface.Draw( _background);
        surface.Draw(_display);
        surface.Draw(_rim);
        //make normalized vector from angle
        
        _cirlceslider1.Draw(surface);
        _cirlceslider2.Draw(surface);
        surface.Draw(_ButtonLabel);
        _SweepButton.Draw(surface);

    }


    class CircularSlider{
        public static bool isDragging = false;
        public Vector2 center;
        public float radius;
        public float angle;
        public float deadzone;
        public TexturedRectangle slider;
        public List<CircularSlider> sidergroup; //slider on same plan in same spot should not  be able to colide with each other

        public enum State{
            Idle,
            hover,
            dragging
        }
        public State state;
        
        public CircularSlider(Vector2 center, float radius, float angle, Texture texture, Vector2  size)
        {
            deadzone = 20f;
            this.center = center;
            this.radius = radius;
            this.angle = angle;
            slider = new TexturedRectangle(center, size, texture, "RadarPanel_slider", true);
        }
        public void Update(MouseState mouseState)
        {
            switch (state)
            {
                case State.Idle:
                    if (checkHover(mouseState))
                    {
                        state = State.hover;
                    }
                    break;
                case State.hover:
                    if (checkHover(mouseState))
                    {
                        if (mouseState.IsButtonDown(MouseButton.Left) && !isDragging)
                        {
                            isDragging = true;
                            state = State.dragging;
                        }
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
                case State.dragging:
                    if (mouseState.IsButtonDown(MouseButton.Left))
                    {
                        var transformed = DrawSystem.DrawSystem.GetMainView().ScreenToViewSpace(new Vector2(mouseState.X, mouseState.Y));
                        var newAngle = Math.Atan2(transformed.Y - center.Y, transformed.X - center.X);
                        newAngle = (float)((newAngle + 2 * Math.PI) % (2 * Math.PI));
                        // Check if the new angle would cause overlap with other sliders
                        foreach (var Slider in sidergroup)
                        {
                            var angleDifference = Math.Abs(Slider.angle - newAngle);
                            // If the angle difference is less than a threshold, adjust the new angle
                            if (angleDifference < Math.PI / deadzone) // Adjust this value as needed
                            {
                                newAngle = Slider.angle + Math.Sign(newAngle - Slider.angle) * Math.PI / deadzone;
                                break;
                            }
                        }

                        angle = (float)newAngle;
                        var self = center + new Vector2((float)Math.Cos(angle) * radius, (float)Math.Sin(angle) * radius);
                    }
                    else
                    {
                        state = State.Idle;
                        isDragging = false;
                    }
                    break;
            }
        }

        private bool checkHover(MouseState mouseState)
        {
            var transformed = DrawSystem.DrawSystem.GetMainView().ScreenToViewSpace(new Vector2(mouseState.X, mouseState.Y));
            // do colision with cirlce defined by slider.drawinfo.position and slider.drawinfo .size.max /2
            if (Vector2.Distance(transformed, slider.drawInfo.Position) < slider.drawInfo.Size.Y*0.9 *0.5)
            {
                   return true;
            }
            return false;
        }
        
        public void Draw(View surface)
        {
            //rostatin 
            slider.drawInfo.Rotation = angle + (float)Math.PI / 2;
            float scale = 1.0f;
            if (state == State.dragging || state == State.hover)
            {
              scale = 1.02f;
            }
          
            slider.drawInfo.Position = center + new Vector2((float)Math.Cos(angle) * radius *scale, (float)Math.Sin(angle) * radius*scale);
            surface.Draw(slider);
            slider.drawInfo.Position = center + new Vector2((float)Math.Cos(angle) * radius , (float)Math.Sin(angle) * radius);
        }
        
    }

    class Button : IDisposable 
    {
        public Vector2 Position;
        public Vector2 Size;
        private TexturedRectangle _buttonON;
        private TexturedRectangle _buttonOFF;
        private TexturedRectangle _buttonOnHover;
        private TexturedRectangle _buttonOffHover;
        private double lastPressTime = 0;
        
        
        
        public enum State
        {
            ON,
            OFF,
            ONHover,
            OFFHover
        }
        public State state;
        
        
        public Button(Vector2 position, Vector2 size, Texture textureON, Texture textureOFF, Texture textureOnHover, Texture textureOffHover)
        {
            Position = position;
            Size = size;
            _buttonON = new TexturedRectangle(Position, Size, textureON);
            _buttonOFF = new TexturedRectangle(Position, Size, textureOFF);
            _buttonOnHover = new TexturedRectangle(Position, Size, textureOnHover);
            _buttonOffHover = new TexturedRectangle(Position, Size, textureOffHover);
            
        }
        public bool isOn()
        {
            if (state == State.ON || state == State.ONHover)
            {
                return true;
            }
            return false;
        }
        
        public void Update (MouseState mouseState,FrameEventArgs args )
        {
            if (lastPressTime > 0)
            {
                lastPressTime -= args.Time;
            }
            
            switch (state)
            {
                case State.ON:
                    if (checkHover(mouseState))
                    {
                        state = State.ONHover;
                    }
                    break;
                case State.OFF:
                    if (checkHover(mouseState))
                    {
                        state = State.OFFHover;
                    }
                    break;
                case State.ONHover:
                    if (checkHover(mouseState))
                    {
                        if (mouseState.IsButtonDown(MouseButton.Left) && lastPressTime <=0)
                        {
                            lastPressTime = 0.5;
                            state = State.OFF;
                        }
                      
                    }
                    else
                    {
                        state = State.ON;
                    }
                    break;
                case State.OFFHover:
                    if (checkHover(mouseState))
                    {
                        if (mouseState.IsButtonDown(MouseButton.Left)  && lastPressTime <=0)
                        {
                            lastPressTime = 0.5;
                            state = State.ON;
                        }
                      
                    }
                    else
                    {
                        state = State.OFF;
                    }
                    break;
            }
            
            
        }
        private bool checkHover(MouseState mouseState)
        {
            var transformed = DrawSystem.DrawSystem.GetMainView().ScreenToViewSpace(new Vector2(mouseState.X, mouseState.Y));
            
            if ( Vector2.Distance(  transformed, Position + Size/2) < Size.X/2)
            {
                return true;
            }
            return false;
        }
        
        public void Draw(View surface)
        {
            switch (state)
            {
                case State.ON:
                    surface.Draw(_buttonON);
                    break;
                case State.OFF:
                    surface.Draw(_buttonOFF);
                    break;
                case State.ONHover:
                    surface.Draw(_buttonOnHover);
                    break;
                case State.OFFHover:
                    surface.Draw(_buttonOffHover);
                    break;
            }
        }
        

        public void Dispose()
        {
            _buttonON.Dispose();
            _buttonOFF.Dispose();
            _buttonOnHover.Dispose();
            _buttonOffHover.Dispose();
        }
    }

    class LinearSlider 
    {
        public Vector2 Begin;
        public Vector2 End;
        public Vector2 Position;
        public float length;
        public TexturedRectangle slider;
        public enum State
        {
            Idle,
            hover,
            dragging
        }
        public State state;
        
        public LinearSlider(Vector2 begin, Vector2 end, Texture texture, Vector2 size)
        {   Position = begin;
            Begin = begin;
            End = end;
            length = Vector2.Distance(begin, end);
            slider = new TexturedRectangle(Begin, size, texture);
        }
        
        public void Update(MouseState mouseState)
        {
            switch (state)
            {
                case State.Idle:
                    if (checkHover(mouseState))
                    {
                        state = State.hover;
                    }
                    break;
                case State.hover:
                    if (checkHover(mouseState))
                    {
                        if (mouseState.IsButtonDown(MouseButton.Left))
                        {
                            state = State.dragging;
                        }
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
                case State.dragging:
                    if (mouseState.IsButtonDown(MouseButton.Left))
                    {
                        
                        // Assuming Slidervec, transformed, and other necessary variables are defined
                        Vector2 Slidervec = End - Begin; // Define Slidervec as the vector from Begin to End
                        Vector2 transformed = DrawSystem.DrawSystem.GetMainView().ScreenToViewSpace(new Vector2(mouseState.X, mouseState.Y)); // Get the transformed point
                        // Step 1: Calculate the perpendicular vector to Slidervec
                        Vector2 perpVector = new Vector2(-Slidervec.Y, Slidervec.X);
                        // Step 2: Calculate the projection of the point transformed onto the perpendicular vector
                        float projectionScalar = Vector2.Dot(transformed, perpVector) / perpVector.LengthSquared;
                        Vector2 projection = perpVector * projectionScalar;
                        // Step 3: Add this projection to the original vector to find the projected point
                        Vector2 projectedPoint = Slidervec + projection;
                        projectedPoint = Vector2.Clamp(projectedPoint, Begin, End);
                        Position = projectedPoint;
                    }
                    else
                    {
                        state = State.Idle;
                    }
                    break;
            }
        }
        
        
        private bool checkHover(MouseState mouseState)
        {
            var transformed = DrawSystem.DrawSystem.GetMainView().ScreenToViewSpace(new Vector2(mouseState.X, mouseState.Y));
            // do colision with cirlce defined by slider.drawinfo.position and slider.drawinfo .size.max /2
            if (Vector2.Distance(transformed, slider.drawInfo.Position) < slider.drawInfo.Size.Y*0.9 *0.5)
            {
                return true;
            }
            return false;
        }
        
        public void Draw(View surface)
        {
            slider.drawInfo.Position = Position;
            surface.Draw(slider);
        }
    }
    
    
}

