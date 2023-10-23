using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Configuration : ScriptableObject
{
    [SerializeField] private StartInventoryConfiguration _startInventoryConfiguration;

    public StartInventoryConfiguration StartInventoryConfiguration
        => _startInventoryConfiguration;
}
