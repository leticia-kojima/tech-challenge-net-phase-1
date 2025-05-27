namespace FCG.Application.Contracts.Catalogs.Queries;

public class GetCatalogQueryRequest : IQuery<CatalogQueryResponse>
{
    public Guid Key { get; set; }
}
