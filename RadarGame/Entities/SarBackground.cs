using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using RadarGame.Physics;

namespace RadarGame.Entities;

public class SarBackground :IEntitie, IDrawObject
{
    public string Name { get; set; }
    private TexturedRectangle background; 
    Shader shader;
    private float time = 0;
    private View target;
    
    
    
    
    public SarBackground()
    {
        Name = "SarBackground";
        target = DrawSystem.DrawSystem.GetView(1);
        Texture texture = new Texture( (int)DrawSystem.DrawSystem.getViewSize(0).X, (int) DrawSystem.DrawSystem.getViewSize(0).Y);
        shader = new Shader("resources/Template/simple_texture.vert", "resources/starshader.frag");
        background = new TexturedRectangle( new Vector2(0,0), DrawSystem.DrawSystem.getViewSize(0),texture, shader);
    }
  
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
       
        time += (float)args.Time;
    }

    public void onDeleted()
    {
        throw new NotImplementedException();
    }

    public void Draw(List<View> surface)
    {
       /*
        * uniform vec2 u_resolution;
          uniform vec2 u_position;
          uniform float u_zoom;
          uniform float u_rotation;
          uniform float u_time;
        */
       //update shader
       shader.Bind();
       
       shader.setUniformV2f( "iResolution", new Vector2(target.Width, target.Height));
         shader.setUniformV2f( "u_position", target.vpossition * 0.005f);
            shader.setUniform1v( "u_zoom", 1);
            shader.setUniform1v( "u_rotation", -target.rotation);
            shader.setUniform1v( "u_time", time);
           // surface[0].rotation = target.rotation;
            
        surface[0].Draw(background);
             
       
    }
}