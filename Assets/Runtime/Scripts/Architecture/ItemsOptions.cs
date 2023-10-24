using System;
using UnityEngine;

[Serializable]
public class ItemsOptions
{
    [SerializeField] private Key[] _singletones;

    public Key[] Singletone
        => _singletones;
}
