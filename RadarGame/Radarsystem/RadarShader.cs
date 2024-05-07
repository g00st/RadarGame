using App.Engine;
using OpenTK.Mathematics;


namespace RadarGame.Radarsystem;

public class RadarShader : Shader
{
    private float AntennaRotation;
    private float distance;
    private Vector2 TextureSize;
    public RadarShader() : base( "resources/Radar/radar_shader.vert","resources/Radar/radar_shader.frag")
    {
    }
    public void setAntennaRotation(float rotation)
    {
        AntennaRotation = rotation;
        this.Bind();
        this.setUniform1v("u_AntennaRotation", AntennaRotation);
    }
    public void setDistance(float d)
    {
        distance = d;
        this.Bind();
        this.setUniform1v("u_Distance", distance);
    }
    public void setTextureSize(Vector2 size)
    {
        TextureSize = size;
        this.Bind();
        this.setUniformV2f("u_TextureSize", TextureSize);
    }
    public void setRadarScreenrange(float RadaScrenrange)
    {
        
        this.Bind();
        this.setUniform1v("u_RadaScrenrange", RadaScrenrange);
    }
    
    public void setRadarRange(float range)
    {
        this.Bind();
        this.setUniform1v("u_RadarRange", range);
    }
}