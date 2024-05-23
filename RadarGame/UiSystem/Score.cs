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
        int scorePoints;
        Score()
        {
            scorePoints = 0;
        }


        public void Update(FrameEventArgs args)
        {
            // zeige hier die points an
        }

        public void Draw(List<View> surface)
        {
            // surface[1].Draw(Background);
            // StartButton.Draw(surface);

        }

        public void AddPoint()
        {
            scorePoints++;
            Console.WriteLine(scorePoints);
        }

        public void Dispose()
        {
            // StartButton.onDeleted();
            // Background.Dispose();

        }
    }
}
