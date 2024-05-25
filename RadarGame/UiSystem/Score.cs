using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using App.Engine;
using App.Engine.Template;
using RadarGame.Entities;

namespace RadarGame.UiSystem
{
    public class Score : IDrawObject, IDisposable
    {
        private int scorePoints;

        Score()
        {
            scorePoints = 0;
        }

        public void Draw(List<View> surface)
        {
            // surface[1].Draw(Background);
            // StartButton.Draw(surface);
            TextRenderer.Write("Score: " + scorePoints, new Vector2(100, 100), new Vector2(30, 30), surface[1], Color4.White);
        }

        public void AddPoint()
        {
            scorePoints++;
            Console.WriteLine(scorePoints);
        }

        public void ScoreReset()
        {
            scorePoints = 0;
        }

        public void Dispose()
        {
            // StartButton.onDeleted();
            // Background.Dispose();

        }
    }
}
