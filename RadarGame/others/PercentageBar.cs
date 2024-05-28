using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;

namespace RadarGame.others;

public class PercentageBar
{
    static private TexturedRectangle barFrame = new TexturedRectangle(new Texture("resources/PercentageBar/frame.png") ,true);
    static private ColoredRectangle bar = new ColoredRectangle( new Vector2(0,0), new Vector2(0,0), new Color4(0,0,0,0.5f), "bar", true);
    public static void  DrawBar(View surface,Vector2 position, float size, float percentage, Color4 color)
    {
        barFrame.drawInfo.Position = position;
        Vector2 size2 = barFrame.drawInfo.Size;
        
        barFrame.drawInfo.Size = size2 * size;
        bar.drawInfo.Position = position = new Vector2( position.X - barFrame.drawInfo.Size.X+ barFrame.drawInfo.Size.X *percentage ,position.Y);
        bar.drawInfo.Size = new Vector2(size2.X * percentage, barFrame.drawInfo.Size.Y);
       // bar.drawInfo.Size = new Vector2(bar.drawInfo.Size.X *0.8f, barFrame.drawInfo.Size.Y * 0.8f);
        
        bar.drawInfo.mesh.Shader.Bind();
        bar.drawInfo.mesh.Shader.setUniform4v( "u_Color", color.R, color.G, color.B, color.A);
         surface.Draw(bar);
         surface.Draw(barFrame);
       
         barFrame.drawInfo.Size = size2 ;
    }
}