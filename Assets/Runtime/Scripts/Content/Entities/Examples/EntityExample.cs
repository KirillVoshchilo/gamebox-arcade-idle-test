public sealed class EntityExample : IEntity
{
    private ExampleType _field;
    public T Get<T>() where T : class
    {
        if (typeof(T) == typeof(ExampleType))
            return _field as T;
        return null;
    }
}

public class ExampleType 
{

}
