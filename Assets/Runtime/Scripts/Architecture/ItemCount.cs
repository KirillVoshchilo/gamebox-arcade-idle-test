using System;
using UnityEngine;

[Serializable]
public class ItemCount
{
    [SerializeField] private Key _name;
    [SerializeField] private int _count;

    public Key Key
        => _name;
    public int Count
        => _count;
}