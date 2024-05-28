using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;




public class GameoverScreen : IEntitie , IDrawObject{

    int[] highscore = new int[5];
    public GameoverScreen()
    {
        StartButton = new Button( 
            DrawSystem.DrawSystem.getViewSize(2)/2 ,
            new Vector2( 200,200),
            new Texture( "resources/Buttons/startbutton_On.png"), 
            new Texture( "resources/Buttons/startbutton_On.png"),
            new Texture( "resources/Buttons/startbutton_OnHover.png"),
            new Texture( "resources/Buttons/startbutton_On.png")
        );
        Name = "GameoverScreen"; 
        Background = new TexturedRectangle( new Vector2(0,0), DrawSystem.DrawSystem.getViewSize(2), new Texture("resources/background2.jpg"));
        highscore = UiSystem.ScoreSystem.getHighscore();
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
        surface[2].Draw(Background);
        StartButton.Draw(surface);
        Vector2 padding = new Vector2(0, 10);
        for (int i = 0; i < highscore.Length; i++)
        {        
            TextRenderer.Write("Highscore" + i + " :" + highscore[i], new Vector2(150,150) + padding, new Vector2(30,30), surface[2], Color4.White);
            padding += new Vector2(0,100);
        }
    }
}