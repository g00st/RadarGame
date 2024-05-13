using App.Engine;

namespace RadarGame;

public interface IDrawObject
{
    public void Draw( List<SubView> surface );
}