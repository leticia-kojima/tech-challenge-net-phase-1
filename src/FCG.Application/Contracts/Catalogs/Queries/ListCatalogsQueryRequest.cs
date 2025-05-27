namespace FCG.Application.Contracts.Catalogs.Queries;

public class ListCatalogsQueryRequest : IQuery<IReadOnlyCollection<CatalogQueryResponse>>
{
    public string? Search { get; set; }
}
