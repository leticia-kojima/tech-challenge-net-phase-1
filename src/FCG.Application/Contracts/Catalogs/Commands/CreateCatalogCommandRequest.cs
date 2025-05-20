using FCG.Application._Common.Contracts;
using FCG.Application.Contracts.Catalogs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class CreateCatalogCommandRequest : CatalogCommandModelBase, ICommand<CreateCatalogCommandResponse>
{
    public CreateCatalogCommandRequest()
    {
        
    }
}
