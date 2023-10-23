using System;
using UnityEngine;

[Serializable]
public class InteractionRequirements
{
    [SerializeField] private ItemCount[] _requirements;

    public ItemCount[] Requirements
        => _requirements;
}