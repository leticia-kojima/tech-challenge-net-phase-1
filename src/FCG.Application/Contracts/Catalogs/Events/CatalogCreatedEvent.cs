using FCG.Application._Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.Contracts.Catalogs.Events;

public class CatalogCreatedEvent : IEvent
{
    public CatalogCreatedEvent(Catalog catalog)
    {
        Catalog = catalog;
    }

    public Catalog Catalog { get; }
}
