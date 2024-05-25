using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class PauseScreen : IEntitie , IDrawObject
{
    private Button StartButton;
    private Button ResumeButton;
    private TexturedRectangle Background;


    public PauseScreen( )
    {
        StartButton = new Button( 
            DrawSystem.DrawSystem.getViewSize(2)/2 ,
            new Vector2( 200,200),
            new Texture( "resources/Buttons/startbutton_On.png"), 
            new Texture( "resources/Buttons/startbutton_On.png"),
            new Texture( "resources/Buttons/startbutton_OnHover.png"),
            new Texture( "resources/Buttons/startbutton_On.png")
        );
        
        ResumeButton = new Button( 
            DrawSystem.DrawSystem.getViewSize(2)/2  + new Vector2(0, 300),
            new Vector2( 200,200),
            new Texture( "resources/Buttons/startbutton_On.png"), 
            new Texture( "resources/Buttons/startbutton_On.png"),
            new Texture( "resources/Buttons/startbutton_OnHover.png"),
            new Texture( "resources/Buttons/startbutton_On.png")
        );
        Name = "PauseScreen"; 
        Background = new TexturedRectangle( new Vector2(0,0), DrawSystem.DrawSystem.getViewSize(2), new Texture("resources/background2.jpg"));
        Name = "PauseScreen"; 
        Background = new TexturedRectangle( new Vector2(0,0), DrawSystem.DrawSystem.getViewSize(2), new Texture("resources/background2.jpg"));
        
        
    }
    
    
    public void Draw(List<View> surface)
    {
        surface[2].Draw(Background);
        StartButton.Draw(surface);
        ResumeButton.Draw(surface);
    } 

    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        StartButton.Update(args, keyboardState, mouseState);
        ResumeButton.Update(args, keyboardState, mouseState);
        if (!StartButton.isOn())
        {
            Console.WriteLine("StartButton Pressed");
            Gamestate.CurrState = Gamestate.State.MainMenu;
           
        }
        if (!ResumeButton.isOn())
        {
            Console.WriteLine("ResumeButton Pressed");
            Gamestate.CurrState = Gamestate.State.Game;
           
        }
    }

    public void onDeleted()
    {
        StartButton.onDeleted();
        Background.Dispose();
        
    }
}
