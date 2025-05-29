namespace FCG.Domain.Catalogs;

public interface ICatalogCommandRepository : ICatalogRepository
{
    Task<bool> ExistByNameAsync(
        string name,
        Guid? ignoreKey = null,
        CancellationToken cancellationToken = default
    );
}
