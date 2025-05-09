using FCG.Domain._Common;
using FCG.Domain.Users;

namespace FCG.Domain.Games;
public class GameDownload : EntityBase
{
    public Guid UserKey { get; private set; }
    public Guid GameKey { get; private set; }

    public virtual User User { get; private set; }
    public virtual Game Game { get; private set; }
}
