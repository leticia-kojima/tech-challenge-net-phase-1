namespace FCG.UnitTests._Common.Abstract;
public class AutoFakerBase<TType> : AutoFaker<TType> where TType : class
{
    public AutoFakerBase()
    {
        CustomInstantiator(f =>
            Activator.CreateInstance(typeof(TType), nonPublic: true) as TType
            ?? throw new InvalidOperationException($"Could not create an instance of {typeof(TType).FullName} with a non-public constructor.")
        );
    }
}
