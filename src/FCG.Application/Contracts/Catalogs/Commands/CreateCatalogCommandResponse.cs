using FCG.Application.Contracts.Catalogs.Abstract;
using FCG.Domain.Catalogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Commands;

public class CreateCatalogCommandResponse : CatalogCommandModelBase
{
    public CreateCatalogCommandResponse(Catalog catalog) : base(catalog)
    {
        Key = catalog.Key;
    }

    public Guid Key { get; set; }
}
