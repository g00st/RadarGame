#version 400 core


out vec4 fragColor;
in vec2 VC; // Vertex coordinates in screen space
uniform vec2 u_TextureSize; // Size of the texture we are rendering to
uniform vec2 u_RadaScrenrange; // Range of the radar screen in meters
uniform float u_Distance; // Distance from antenne of radar point intersection
uniform float u_AntennaRotation; // Rotation of the radar antenne 
void main()
{
    vec4 newColor = vec4(0.0, 0.0, 0.0, 1.0);
    // Calculate the distance from the center of the texture
    float distance = length(VC - u_TextureSize / 2.0);
    //Clucualte distance in meters
    float distanceMeters = distance * u_RadaScrenrange.x / (u_TextureSize.x / 2.0);
     
    
    
    
  
    
    
    
    fragColor = newColor;
}