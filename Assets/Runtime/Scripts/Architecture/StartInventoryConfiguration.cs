using System;
using UnityEngine;

[Serializable]
public class StartInventoryConfiguration
{
    [SerializeField] private ItemCount[] _items;

    public ItemCount[] Items
        => _items;
}
