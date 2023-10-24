using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Configuration : ScriptableObject
{
    [SerializeField] private StartInventoryConfiguration _startInventoryConfiguration;
    [SerializeField] private ItemsOptions _itemsOptions;
  
    public StartInventoryConfiguration StartInventoryConfiguration
        => _startInventoryConfiguration;
    public ItemsOptions ItemsOptions 
        => _itemsOptions; 
}
