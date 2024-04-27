#version 400 core


out vec4 fragColor;
in vec2 VC; // Vertex coordinates in screen space
uniform vec2 u_TextureSize; // Size of the texture we are rendering to
uniform float u_Distance; // Distance from antenne of radar point intersection
uniform float u_AntennaRotation; // Rotation of the radar antenne 
void main()
{
    vec4 newColor = vec4(0.0, 0.0, 0.0, 1.0);
    // Calculate the distance from the center of the texture
    float distance = length(VC - u_TextureSize / 2.0);
    // Calculate if point is inside Antenna ilumination area
    vec2 antennaDirection = vec2(cos(u_AntennaRotation), sin(u_AntennaRotation)) + u_TextureSize / 2.0;
    // Calculate if current texle is inside the radar ilumination area
    float dotProduct = dot(normalize(VC - u_TextureSize / 2.0), normalize(antennaDirection - u_TextureSize / 2.0));
    if (distance < u_Distance )
    {
        // Calculate the color of the point
        float intensity = 1.0 - distance / u_Distance;
        newColor = vec4(1, 1, 1, 1.0);
    }
    else
    {
        newColor = vec4(0.0, 0.0, 0.0, 1.0);
    }
    
    
    fragColor = newColor;
}