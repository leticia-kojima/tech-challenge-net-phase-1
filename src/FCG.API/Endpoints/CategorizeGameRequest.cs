using System;

namespace FCG.API.Endpoints;

public class CategorizeGameRequest
{
    public required Guid CatalogKey { get; set; }
}
