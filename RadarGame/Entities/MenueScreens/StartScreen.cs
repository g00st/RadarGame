using App.Engine;
using App.Engine.Template;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace RadarGame.Entities;

public class StartScreen : IDrawObject ,IEntitie
{
    private Button StartButton;
    private TexturedRectangle Background;
    private int counter = 0;


    public StartScreen( )
    {
         StartButton = new Button( 
            DrawSystem.DrawSystem.getViewSize(2)/2 ,
            new Vector2( 100,100),
            new Texture( "resources/Buttons/startbutton_On.png"), 
            new Texture( "resources/Buttons/startbutton_On.png"),
            new Texture( "resources/Buttons/startbutton_OnHover.png"),
            new Texture( "resources/Buttons/startbutton_On.png")
        );
       
         
        Name = "Startscreen"; 
        
        Background = new TexturedRectangle( new Vector2(0,0), DrawSystem.DrawSystem.getViewSize(2), new Texture("resources/background2.jpg"));
        
        
    }
    
    
    public void Draw(List<View> surface)
    {
        surface[2].Draw(Background);
        StartButton.Draw(surface);
        
        TextRenderer.Write("00000 abcdefgABCDEFG Hello 123" + counter, new Vector2(100, 100), new Vector2(30, 30), surface[2], Color4.White);
    } 

    public string Name { get; set; }
    public void Update(FrameEventArgs args, KeyboardState keyboardState, MouseState mouseState)
    {
        counter++;
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
        
    }
}