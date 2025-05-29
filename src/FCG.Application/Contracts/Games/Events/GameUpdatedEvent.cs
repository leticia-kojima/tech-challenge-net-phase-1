using FCG.Domain.Games;

namespace FCG.Application.Contracts.Catalogs.Events;

public class GameUpdatedEvent : IEvent
{
    public GameUpdatedEvent(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
