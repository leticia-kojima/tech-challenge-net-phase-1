using FCG.Domain.Catalogs;
using FCG.Infrastructure._Common.Interfaces;

namespace FCG.Infrastructure.Seeders;
public class CatalogSeeder : ISeeder<Catalog>
{
    public IReadOnlyCollection<Catalog> GetData() => new List<Catalog> {
        new (new ("1cb7a523-7866-47c8-90a5-bdec86cc02a0"), "REST and RESTful", "Content related to the REST and RESTful..."),
        new (new ("69c72c65-d036-4fa4-8f84-7cd37449a009"), "AWS Cloud", "Cloud solutions with AWS..."),
        new (new ("82b63727-03b4-48db-9e5d-cde73ee9c68a"), "Azure Cloud", "Cloud solutions with Azure..."),
        new (new ("d5460750-bb4d-40e8-9de5-c28563043e36"), "Programming Languages", "C#, Java..."),
    };
}
