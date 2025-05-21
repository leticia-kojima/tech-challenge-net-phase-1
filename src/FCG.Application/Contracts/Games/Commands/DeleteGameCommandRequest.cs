namespace FCG.Application.Contracts.Games.Commands;
public class DeleteGameCommandRequest : IRequest
{
    public required Guid Key { get; set; }
}

