using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public sealed class IconsConfiguration : ScriptableObject
{
    [SerializeField] private Icon[] _icons;

    private Dictionary<string, Icon> _iconsDictionary = new();

    public Sprite this[string value]
        => _iconsDictionary[value].Value;
    public void Construct()
    {
        foreach (Icon icon in _icons)
            _iconsDictionary.Add(icon.Name.Value, icon);
    }
}
