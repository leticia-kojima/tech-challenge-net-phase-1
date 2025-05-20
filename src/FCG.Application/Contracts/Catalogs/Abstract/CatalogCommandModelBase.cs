using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCG.Domain.Users;

namespace FCG.Application.Contracts.Catalogs.Abstract;

public abstract class CatalogCommandModelBase
{
    protected CatalogCommandModelBase()
    {            
    }

    protected CatalogCommandModelBase(Catalog catalog)
    {
        Name = catalog.Name;
        Description = catalog.Description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}
