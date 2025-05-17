namespace FCG.Domain._Common.Abstract;
public abstract class ValueObjectBase<TObject>
    where TObject : ValueObjectBase<TObject>
{
    public static bool operator ==(ValueObjectBase<TObject> left, ValueObjectBase<TObject> right)
    {
        if (left is null && right is null) return true;
        if (left is null || right is null) return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObjectBase<TObject> left, ValueObjectBase<TObject> right)
    {
        return !(left == right);
    }

    public override bool Equals(object obj)
    {
        if (obj is not TObject other) return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x?.GetHashCode() ?? 0)
            .Aggregate((x, y) => x ^ y);
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}
