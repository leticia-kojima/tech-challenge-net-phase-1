using FCG.Application.Contracts.Catalogs.Abstract;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class CreateCatalogCommandRequest : CatalogCommandModelBase, ICommand<CreateCatalogCommandResponse>
{
    public CreateCatalogCommandRequest()
    {
        
    }
}
