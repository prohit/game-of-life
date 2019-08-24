using System;

public static class EventManager
{
    static Action<GameState> gameStateChangeEvent;

    public static void AddGameStateChangeEvent(Action<GameState> pEvent) => gameStateChangeEvent += pEvent;
    public static void RemoveGameStateChangeEvent(Action<GameState> pEvent)
    {
        if(gameStateChangeEvent != null)
        {
            gameStateChangeEvent -= pEvent;
        }
    }

    public static void TriggerGameGameStateEvent(GameState state)
    {
        gameStateChangeEvent?.Invoke(state);
    }
}
