/// <summary>
/// Интерфейс определяет сущность.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Получить компонент сущности.
    /// </summary>
    T Get<T>() where T : class;
}
