namespace RadarGame;

public class Gamestate
{
    private static State _currState;
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
        switch (neState)
        {
            case State.MainMenu:
                Entities.EntityManager.ClearObjects();
                //TODO: Entities.EntityManager.AddObject( Startscreen elementens);
                break;
            case State.Game:
                if (CurrState == State.MainMenu )
                {
                    //TODO: Entities.EntityManager.Remove( Startscreen elementents);
                    // TODO: create new game elements
                    // TODO: Entities.EntityManager.AddObject( Game elementents);
                }
                else if (CurrState == State.Pause)
                {
                    //Todo: Entities.EntityManager.AddObject( Game elementents);
                    //Todo: Entities.EntityManager.Remove( Pause elementents);
                }

                break;
            case State.Pause:
                        //TODO: Entities.EntityManager.AddObject( Pause elementen);
                        //TODO: Entities.EntityManager.Remove( Game elementen);
                break;
            case State.GameOver:
                     //TODO: Entities.EntityManager.AddObject( GameOver elementen);
                     //TODO: Entities.EntityManager.Delete( Game elementen);
                break;
        }
        _currState = neState;
    }
}

