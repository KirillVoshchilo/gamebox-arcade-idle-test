/// <summary>
/// Интерфейс для компоненты сущности.
/// </summary>
public interface IComp<T>
{
    /// <summary>
    /// Вернуть этот компонент, как представитель класса.
    /// </summary>
    T This();
}
