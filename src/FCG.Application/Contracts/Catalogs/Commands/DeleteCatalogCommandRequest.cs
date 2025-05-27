namespace FCG.Application.Contracts.Catalogs.Commands;

public class DeleteCatalogCommandRequest : ICommand
{
    public Guid Key { get; set; }
}
