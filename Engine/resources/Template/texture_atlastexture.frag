#version 400 core

out vec4 fragColor;
uniform sampler2D tex;
uniform int index;
uniform vec2 texturesize;    // Size of the entire texture atlas
uniform vec2 subimagesize;   // Size of one subimage in the atlas
in vec2 textCords;           // Input texture coordinates

void main()
{
    // Calculate the number of subimages in the atlas (1D array)
    float numSubImages = texturesize.x / subimagesize.x;

    // Calculate the horizontal offset for the subimage
    float subImageOffset = float(index) * subimagesize.x;

    // Adjust the input texture coordinates
    vec2 adjustedCoords = vec2(textCords.x * subimagesize.x + subImageOffset, textCords.y * subimagesize.y);
    
    // Normalize the adjusted coordinates to the range [0, 1]
    vec2 normalizedCoords = adjustedCoords / texturesize;

    // Sample the texture atlas using the normalized coordinates
    fragColor = texture(tex, normalizedCoords);
}
