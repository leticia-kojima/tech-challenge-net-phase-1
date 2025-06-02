using FCG.Domain.Users;

namespace FCG.Domain.Games;
public class GameEvaluation : EntityBase
{
    protected GameEvaluation() : base() { }

    public GameEvaluation(
        EFiveStars stars,
        string? comment,
        Guid userKey,
        Guid gameKey
    )
    {
        Stars = stars;
        Comment = comment;
        UserKey = userKey;
        GameKey = gameKey;
    }

    public EFiveStars Stars { get; private set; }
    public string? Comment { get; private set; }
    public Guid UserKey { get; private set; }
    public Guid GameKey { get; private set; }    public virtual User? User { get; private set; }
    public virtual Game? Game { get; private set; }
}
