using System;
using System.Collections.Generic;

/// <summary>
/// ������ ���������.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AStateMachine<T> where T : Enum
{
    protected List<IStateObserver<T>> _observers = new();
    protected T _currentState;

    public T CurrentState
        => _currentState;

    public void AddObservers(IEnumerable<IStateObserver<T>> observers)
    {
        foreach (IStateObserver<T> observer in observers)
            _observers.Add(observer);
    }
    public void AddObserver(IStateObserver<T> observer)
        => _observers.Add(observer);
    public void RemoveObserver(IStateObserver<T> observer)
        => _observers.Remove(observer);
    /// <summary>
    /// ������� � ���������. � ���� ������ ��� ���������� ����������.
    /// </summary>
    public abstract void GoTo(T state);
}

/// <summary>
/// ����������� �� ����������.
/// </summary>
public interface IStateObserver<T>
{
    void OnStateChanged(T appState);
}