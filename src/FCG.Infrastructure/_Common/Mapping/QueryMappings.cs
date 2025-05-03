namespace FCG.Infrastructure._Common.Mapping;
public static class QueryMappings
{
    public static void RegisterMappings()
    {
        var mappings = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => typeof(QueryMappingBase<>).IsAssignableFrom(t))
            .ToList();

        mappings.ForEach(m => Activator.CreateInstance(m));
    }
}
