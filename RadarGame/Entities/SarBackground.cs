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
        
        var size = (int )Math.Sqrt( target.vsize.X* target.vsize.X + target.vsize.Y * target.vsize.Y);
        Texture texture = new Texture( size,size);
        shader = new Shader("resources/Template/simple_texture.vert", "resources/starshader.frag");
        background = new TexturedRectangle(target.vpossition  , new Vector2(size),texture, shader, true);
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
         shader.setUniformV2f( "u_position", target.vpossition * 0.1f);
            shader.setUniform1v( "u_zoom", 1 + target.vsize.X* 0.000005f);
            shader.setUniform1v( "u_rotation", 0);
            shader.setUniform1v( "u_time", time); 
        //rotate 180
        //extreme scuff aber funktioniert 
        background.drawInfo.Rotation =  target.rotation + (float) Math.PI;  ;
        
        surface[0].Draw(background);
             
       
    }
}