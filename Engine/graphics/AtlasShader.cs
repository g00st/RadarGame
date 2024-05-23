using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Engine;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.graphics
{
    public class AtlasShader : Shader
    {
        public AtlasShader() : base("resources/Template/simple_texture.vert", "resources/Template/texture_atlastexture.frag")
        {

        }
    }
}
