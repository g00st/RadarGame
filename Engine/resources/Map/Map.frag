#version 400 core
out vec4 fragColor;
in vec2 textCords;
uniform float random;
uniform sampler2D tex;
void main()
{
  //  fragColor =   vec4(textCords, 0.0, 1.0);
  //distance of center of the screen
  float distance = length(textCords - vec2(0.5, 0.5));
  vec3 color = texture(tex, textCords*0.2 + random*0.5).rgb;
   
  fragColor =  vec4(color* (1.0 - distance*1.9), 1.0) ;
}