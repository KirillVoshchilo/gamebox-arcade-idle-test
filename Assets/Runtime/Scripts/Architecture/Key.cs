using UnityEngine;

[CreateAssetMenu]
public sealed class Key : ScriptableObject
{
    [SerializeField] private string _name;

    public string Value 
        => _name; 
}