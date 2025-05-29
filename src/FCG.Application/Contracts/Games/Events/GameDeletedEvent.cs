using FCG.Domain.Games;

namespace FCG.Application.Contracts.Catalogs.Events;

public class GameDeletedEvent : IEvent
{
    public GameDeletedEvent(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
