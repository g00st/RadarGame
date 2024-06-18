using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System.Collections.Generic;
using System;

namespace RadarGame.Entities
{
    public class StartScreen : IDrawObject, IEntitie
    {
        private Button StartButton;
        private TexturedRectangle Background;
        private Shader _shader;
        private float _time;
        private int counter = 0;
        private View _target;

        public StartScreen()
        {
            StartButton = new Button(
                DrawSystem.DrawSystem.getViewSize(2) / 2,
                new Vector2(100, 100),
                new Texture("resources/Buttons/startbutton_On.png"),
                new Texture("resources/Buttons/startbutton_On.png"),
                new Texture("resources/Buttons/startbutton_OnHover.png"),
                new Texture("resources/Buttons/startbutton_On.png")
            );
            Name = "Startscreen";
            _target = DrawSystem.DrawSystem.GetView(2);
            _shader = new Shader("resources/Background/star_bg.vert", "resources/Background/star_bg.frag");
            Background = new TexturedRectangle(Vector2.Zero, new Vector2(_target.vsize.X, _target.vsize.Y), null, _shader, true);
        }

        public void Draw(List<View> surface)
        { 
            Background.drawInfo.Position = new Vector2(surface[2].vsize.X/2, surface[2].vsize.Y/2);
            Background.drawInfo.Size = new Vector2(surface[2].vsize.X, surface[2].vsize.Y); // Ensure the size matches the surface
            _shader.Bind();
            _shader.setUniformV2f("iResolution", new Vector2(surface[2].vsize.X, surface[2].vsize.Y));
            _shader.setUniform1v("iTime", _time); 
            
            surface[2].Draw(Background);
            _shader.Unbind();

            StartButton.Draw(surface);
            TextRenderer.Write("00000 abcdefgABCDEFG Hello 123" + counter, new Vector2(100, 100), new Vector2(30, 30), surface[2], Color4.White);
        }

        public string Name { get; set; }
        public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
        {
            counter++;
            _time += (float)args.Time;
            StartButton.Update(args, keyboardState, mouseState);
            if (!StartButton.isOn())
            {
                Console.WriteLine("StartButton Pressed");
                Gamestate.CurrState = Gamestate.State.Game;
            }
        }

        public void onDeleted()
        {
            StartButton.onDeleted();
            Background.Dispose();
            _shader.Dispose();
        }
    }
}
