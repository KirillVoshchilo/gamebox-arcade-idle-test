using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopProductButton : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Button _button;

    private Key _key;
    private readonly SEvent<Key> _onButtonClick = new();

    public Sprite Icon
    {
        get => _iconImage.sprite;
        set => _iconImage.sprite = value;
    }
    public string Name
    {
        get => _itemName.text;
        set => _itemName.text = value;
    }
    public Key Key
    {
        get => _key;
        set => _key = value;
    }
    public SEvent<Key> OnButtonClick
        => _onButtonClick;

    public void Construct()
    {
        _button.onClick.AddListener(()
            => _onButtonClick.Invoke(_key));
    }
}