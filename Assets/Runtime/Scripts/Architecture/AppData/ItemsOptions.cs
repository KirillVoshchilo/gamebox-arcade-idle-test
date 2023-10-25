using System;
using UnityEngine;

[Serializable]
public sealed class ItemsOptions
{
    [SerializeField] private Key[] _singletones;

    public Key[] Singletone
        => _singletones;
}
