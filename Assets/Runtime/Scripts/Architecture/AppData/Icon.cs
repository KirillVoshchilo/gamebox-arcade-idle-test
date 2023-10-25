using System;
using UnityEngine;

[Serializable]
public sealed class Icon
{
    [SerializeField] private Key _name;
    [SerializeField] private Sprite _icon;

    public Key Name
        => _name;
    public Sprite Value
        => _icon;
}