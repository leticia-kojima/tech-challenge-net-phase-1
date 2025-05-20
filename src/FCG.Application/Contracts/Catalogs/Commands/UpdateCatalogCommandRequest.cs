using FCG.Application._Common.Contracts;
using FCG.Application.Contracts.Catalogs.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class UpdateCatalogCommandRequest : CatalogCommandModelBase, ICommand<UpdateCatalogCommandResponse>
{
    public UpdateCatalogCommandRequest()
    {        
    }

    public Guid Key { get; set; }
}
