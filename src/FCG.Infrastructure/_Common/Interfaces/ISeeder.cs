namespace FCG.Infrastructure._Common.Interfaces;
public interface ISeeder<T> where T : class
{
    public IReadOnlyCollection<T> GetData();
}
