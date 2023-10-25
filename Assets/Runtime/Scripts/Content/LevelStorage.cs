using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class LevelStorage : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private GameObject[] _autoInjectObjects;

    private LifetimeScope _container;
    private HashSet<IDestructable> _destructables = new();


    public Transform PlayerTransform
        => _playerTransform;

    public void Construct(LifetimeScope lifeTimeScope)
    {
        _container = lifeTimeScope;
        AutoInjectAll(lifeTimeScope);
    }
    public void Destruct()
    {
        foreach (IDestructable destructable in _destructables)
            destructable.Destruct();
    }

    void AutoInjectAll(LifetimeScope lifeTimeScope)
    {
        if (_autoInjectObjects == null)
            return;

        foreach (GameObject target in _autoInjectObjects)
        {
            if (target != null) // Check missing reference
            {
                _destructables.UnionWith(target.GetComponents<IDestructable>());
                _destructables.UnionWith(target.GetComponentsInChildren<IDestructable>());
                lifeTimeScope.Container.InjectGameObject(target);
            }
        }
    }
}
