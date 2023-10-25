public interface IEntity
{
    T Get<T>() where T : class;
}
