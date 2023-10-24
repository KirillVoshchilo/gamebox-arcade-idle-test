using System;
using UnityEngine;

[Serializable]
public sealed class ItemCost
{
    [SerializeField] private Key _key;
    [SerializeField] private ItemCount[] _cost;

    public Key Key 
        => _key; 
    public ItemCount[] Cost 
        => _cost; 
}