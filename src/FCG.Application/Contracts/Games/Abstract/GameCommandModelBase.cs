using FCG.Domain.Games;

namespace FCG.Application.Contracts.Games.Abstract;
public abstract class GameCommandModelBase
{
    protected GameCommandModelBase()
    {
    }

    protected GameCommandModelBase(Game game)
    {
        Title = game.Title;
        Description = game.Description;
    }

    public string Title { get; set; }
    public string Description { get; set; }
}
