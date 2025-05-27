using FCG.Application.Contracts.Catalogs.Abstract;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class UpdateCatalogCommandRequest : CatalogCommandModelBase, ICommand<UpdateCatalogCommandResponse>
{
    public UpdateCatalogCommandRequest()
    {        
    }

    public Guid Key { get; set; }
}
