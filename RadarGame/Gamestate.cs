using App.Engine;
using OpenTK.Mathematics;
using RadarGame.Entities;

namespace RadarGame;

public class Gamestate
{
    private static State _currState;
    private static List<IEntitie> currentEntities = new List<IEntitie>();
    public  static State CurrState
    {
        get => _currState;
         set => onStateChange(value);}
    public enum State
    {
        MainMenu,
        Game,
        Pause,
        GameOver
    }
    
    
    public static void onStateChange( State neState)
    {
       
        if (neState == _currState)
        {
            return;
        } 
        Console.WriteLine("State Changed to: " + neState);
        switch (neState)
        {
            case State.MainMenu:
               EntityManager.ClearObjects();
               EntityManager.AddObject(new StartScreen());
                //TODO: Entities.EntityManager.AddObject( Startscreen elementens);
                break;
            case State.Game:
                if (CurrState == State.MainMenu )
                {
                    EntityManager.DeleteObject( EntityManager.GetObject("Startscreen"));
                    EntityManager.AddObject(new cursor());
                    EntityManager.AddObject(new PlayerObject( Vector2.Zero, 0f, "Player"));
                    EntityManager.AddObject( new cursor( "cursor4"));
                    EntityManager.AddObject(new CompasPanel( DrawSystem.DrawSystem.getViewSize(1) - new Vector2(200, 200), new Vector2(150, 150), "CompasPanel"));
                    EntityManager.AddObject(new Pauser());
                    for (int i = 0; i < 100; i++)
                    {
                        GameObject gameObject = new GameObject( Vector2.One, 0f, "test"+i, 0);
                        EntityManager.AddObject(gameObject);
                    }
                    Button testButton;
                    var Size = DrawSystem.DrawSystem.getViewSize(1);
                    testButton = new Button(Size - new Vector2(Size.X / 6.4f), new Vector2(Size.X / 16),
                        new Texture("resources/Buttons/pausebutton_On.png"), new Texture("resources/Buttons/pausebutton_Off.png"),
                        new Texture("resources/Buttons/pausebutton_onHover.png"), new Texture("resources/Buttons/pausebutton_onHover.png"));
                    testButton.Name = "testButton";
                    EntityManager.AddObject(testButton);
                    
                    
                }
                else if (CurrState == State.Pause)
                {
                    foreach (var g in currentEntities)
                    {
                        Console.WriteLine("Adding Object: " + g.Name);
                        EntityManager.AddObject(g);
                    }
                    EntityManager.DeleteObject( EntityManager.GetObject("PauseScreen"));
                   
                }

                break;
            case State.Pause:
                        currentEntities = EntityManager.GetObjects();
                        
                        EntityManager.RemoveAllObjects();
                        EntityManager.AddObject(new PauseScreen());
                      
                break;
            case State.GameOver:
                      EntityManager.ClearObjects();
                      EntityManager.AddObject(new GameoverScreen());
                   
                break;
        }
        _currState = neState;
    }
}

