using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using App.Engine;
using App.Engine.Template;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities
{
    public class Button : IDrawObject, IEntitie
    {
        public Vector2 Position;
        public Vector2 Size;
        private TexturedRectangle _buttonON;
        private TexturedRectangle _buttonOFF;
        private TexturedRectangle _buttonOnHover;
        private TexturedRectangle _buttonOffHover;
        private double lastPressTime = 0;

        // Example Button:
        // _SweepButton = new Button(Position +Size - new Vector2(Size.X/6.4f), new Vector2(Size.X /16),
        // new Texture("resources/Radar/Button_on.png"), new Texture("resources/Radar/Button_of.png"),
        // new Texture("resources/Radar/Button_on_hover.png"), new Texture("resources/Radar/Button_of_hover.png"));
        // _SweepButton.Name = "sweep";

        public string Name { get; set; }

        public enum State
        {
            ON,
            OFF,
            ONHover,
            OFFHover
        }
        public State state;

        public Button(Vector2 position, Vector2 size, Texture textureON, Texture textureOFF, Texture textureOnHover, Texture textureOffHover)
        {
            Position = position;
            Size = size;
            _buttonON = new TexturedRectangle(Position, Size, textureON);
            _buttonOFF = new TexturedRectangle(Position, Size, textureOFF);
            _buttonOnHover = new TexturedRectangle(Position, Size, textureOnHover);
            _buttonOffHover = new TexturedRectangle(Position, Size, textureOffHover);
            

        }
        public bool isOn()
        {
            if (state == State.ON || state == State.ONHover)
            {
                return true;
            }
            return false;
        }

        public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
        {
            if (lastPressTime > 0)
            {
                lastPressTime -= args.Time;
            }

            switch (state)
            {
                case State.ON:
                    if (checkHover(mouseState))
                    {
                        state = State.ONHover;
                    }
                    break;
                case State.OFF:
                    if (checkHover(mouseState))
                    {
                        state = State.OFFHover;
                    }
                    break;
                case State.ONHover:
                    if (checkHover(mouseState))
                    {
                        if (mouseState.IsButtonDown(MouseButton.Left) && lastPressTime <= 0)
                        {
                            lastPressTime = 0.5;
                            state = State.OFF;
                        }

                    }
                    else
                    {
                        state = State.ON;
                    }
                    break;
                case State.OFFHover:
                    if (checkHover(mouseState))
                    {
                        if (mouseState.IsButtonDown(MouseButton.Left) && lastPressTime <= 0)
                        {
                            lastPressTime = 0.5;
                            state = State.ON;
                        }

                    }
                    else
                    {
                        state = State.OFF;
                    }
                    break;
            }


        }
        private bool checkHover(MouseState mouseState)
        {
            var transformed = DrawSystem.DrawSystem.ScreenToWorldcord(new Vector2(mouseState.X, mouseState.Y), 1);

            if (Vector2.Distance(transformed, Position + Size / 2) < Size.X / 2)
            {
                return true;
            }
            return false;
        }

        public void Draw(List<View> surface)
        {
            switch (state)
            {
                case State.ON:
                    surface[1].Draw(_buttonON);
                    break;
                case State.OFF:
                    surface[1].Draw(_buttonOFF);
                    break;
                case State.ONHover:
                    surface[1].Draw(_buttonOnHover);
                    break;
                case State.OFFHover:
                    surface[1].Draw(_buttonOffHover);
                    break;
            }
        }


        public void onDeleted()
        {
            _buttonON.Dispose();
            _buttonOFF.Dispose();
            _buttonOnHover.Dispose();
            _buttonOffHover.Dispose();
        }
    }
}
