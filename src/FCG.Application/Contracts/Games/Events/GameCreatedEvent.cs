using FCG.Domain.Games;

namespace FCG.Application.Contracts.Catalogs.Events;

public class GameCreatedEvent : IEvent
{
    public GameCreatedEvent(Game game)
    {
        Game = game;
    }

    public Game Game { get; }
}
