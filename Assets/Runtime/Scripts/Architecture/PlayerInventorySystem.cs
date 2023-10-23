using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerInventorySystem
{
    private readonly Dictionary<string, int> _inventory = new();
    private readonly SEvent<string, int> _onInventoryValueChanged = new();

    public SEvent<string, int> OnInventoryValueChanged
        => _onInventoryValueChanged;
    public List<(string, int)> AllItems
    {
        get
        {
            List<(string name, int count)> items = new();
            foreach (KeyValuePair<string, int> item in _inventory)
                items.Add((name: item.Key, count: item.Value));
            return items;
        }
    }

    public int GetCount(string name)
    {
        if (_inventory.TryGetValue(name, out int value))
            return value;
        return 0;
    }
    public void AddItem(string name, int count)
    {
        if (_inventory.TryGetValue(name, out int value))
            _inventory[name] = count + value;
        else _inventory.Add(name, count);
        _onInventoryValueChanged.Invoke(name, _inventory[name]);
        Debug.Log($"В инвентаре стало {name}: {_inventory[name]}");
    }
    public void RemoveItem(string name, int count)
    {
        if (_inventory.TryGetValue(name, out int value))
            _inventory[name] = count + value;
        else Debug.Log("Пытаешся отнять то, чего нет");
        _onInventoryValueChanged.Invoke(name, _inventory[name]);
    }
}