using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TriggerComponent : MonoBehaviour
{
    private readonly SEvent<Collider> _onEnter = new();
    private readonly SEvent<Collider> _onExit = new();

    public SEvent<Collider> OnEnter
        => _onEnter;
    public SEvent<Collider> OnExit
        => _onExit;

    private void OnTriggerEnter(Collider other)
    {
        _onEnter.Invoke(other);
    }
    private void OnTriggerExit(Collider other)
    {
        _onExit.Invoke(other);
    }
}
