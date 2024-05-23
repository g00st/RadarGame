using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;




public class GameoverScreen : IEntitie , IDrawObject{
    
    public GameoverScreen()
    {
        StartButton = new Button( 
            DrawSystem.DrawSystem.getViewSize(1)/2 ,
            new Vector2( 200,200),
            new Texture( "resources/Buttons/startbutton_On.png"), 
            new Texture( "resources/Buttons/startbutton_On.png"),
            new Texture( "resources/Buttons/startbutton_OnHover.png"),
            new Texture( "resources/Buttons/startbutton_On.png")
        );
        Name = "GameoverScreen"; 
        Background = new TexturedRectangle( new Vector2(0,0), DrawSystem.DrawSystem.getViewSize(1), new Texture("resources/background2.jpg"));
    }
    private Button StartButton;
    private TexturedRectangle Background;


    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        StartButton.Update(args, keyboardState, mouseState);
        if (!StartButton.isOn())
        {
            Console.WriteLine("StartButton Pressed");
            Gamestate.CurrState = Gamestate.State.MainMenu;
           
        }
    }

    public void onDeleted()
    {
        StartButton.onDeleted();
        Background.Dispose();
    }

    public void Draw(List<View> surface)
    {
        surface[1].Draw(Background);
        StartButton.Draw(surface);
    }
}