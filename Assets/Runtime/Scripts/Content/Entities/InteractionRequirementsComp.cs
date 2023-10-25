using System;
using UnityEngine;

[Serializable]
public sealed class InteractionRequirementsComp
{
    [SerializeField] private ItemCount[] _requirements;

    public ItemCount[] Requirements
        => _requirements;
}