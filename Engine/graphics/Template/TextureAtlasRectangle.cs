using App.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Engine.graphics.Template
{
    public class TextureAtlasRectangle : DrawObject, IDisposable
    {
        private static VAO _sharedVAOcenter = null;
        private static VAO _sharedVAO = null;
        private static Shader _sharedShader = new AtlasShader();

        public TextureAtlasRectangle(Vector2 position, Vector2 size, Texture? texture, string name = "TexturedRectangle")
        {
            this.drawInfo = new DrawInfo(position, size, 0, null, name);

            if (_sharedVAOcenter == null)
            {
                Bufferlayout bufferlayout = new Bufferlayout();
                bufferlayout.count = 2;
                bufferlayout.normalized = false;
                bufferlayout.offset = 0;
                bufferlayout.type = VertexAttribType.Float;
                bufferlayout.typesize = sizeof(float);
                _sharedVAOcenter = new VAO();
                _sharedVAOcenter.LinkAtribute(new float[] { -0.5f, -0.5f, 0.5f, -0.5f, 0.5f, 0.5f, -0.5f, 0.5f }, bufferlayout);
                _sharedVAOcenter.LinkAtribute(new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f, 0.0f, 1.0f }, bufferlayout);
                _sharedVAOcenter.LinkElements(new uint[] { 0, 1, 2, 2, 3, 0 });
            }
            this.drawInfo.mesh = new Mesh(_sharedVAOcenter, 4, new uint[] { 0, 1, 2, 2, 3, 0 }, noCleanUp: true);

            this.drawInfo.mesh.Shader = _sharedShader;
            if (texture != null) this.drawInfo.mesh.Texture = texture;

        }

        public DrawInfo drawInfo { get; }

        public void Dispose()
        {
            drawInfo.Dispose();
        }
    }
}
