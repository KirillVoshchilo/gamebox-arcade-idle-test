using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AScriptableFactory : ScriptableObject
{
    private Transform _parent;

    public Transform Parent
    {
        get => _parent;
        set => _parent = value;
    }

    public abstract void Create();
    public abstract void Remove(GameObject obj);
}
