using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Mathematics;

namespace RadarGame.SoundSystem
{
    public interface SoundObject
    {
        bool isDirectional { get; }
        Vector2 Position { get; }
        /*
         * DebugColoredRectangle = new TexturedRectangle(
            position,
            new OpenTK.Mathematics.Vector2(50f, 50f), 
            new Texture("resources/cirno.png"),
            Name,
            true
            );
         */
    }
}
