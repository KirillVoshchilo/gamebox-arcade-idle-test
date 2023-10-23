using UnityEngine;

[CreateAssetMenu]
public sealed class Icon : ScriptableObject
{
    [SerializeField] private Key _name;
    [SerializeField] private Sprite _icon;

    public Key Name
        => _name;
    public Sprite Value
        => _icon;
}