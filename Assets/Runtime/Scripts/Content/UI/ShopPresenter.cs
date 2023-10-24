using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class ShopPresenter : MonoBehaviour
{
    [SerializeField] private Transform _productsContainer;
    [SerializeField] private Transform _requirementsContainer;
    [SerializeField] private RequirementItemPresenter _requirementPrefab;
    [SerializeField] private ShopProductButton _shopProductButtonPrefab;
    [SerializeField] private Button _buyButton;

    private readonly List<RequirementItemPresenter> _itemsList = new();
    private readonly List<ShopProductButton> _productsList = new();
    private readonly Dictionary<string, ItemCost> _itemsCost = new();
    private IconsConfiguration _iconsConfiguration;
    private Key _selectedProduct;
    private Configuration _configurations;
    private PlayerInventorySystem _playerInventorySystem;

    [Inject]
    public void Construct(IconsConfiguration iconsConfiguration,
        PlayerInventorySystem playerInventorySystem,
        Configuration configuration)
    {
        _configurations = configuration;
        _playerInventorySystem = playerInventorySystem;
        _buyButton.onClick.AddListener(OnBuyButtonPressed);
        _iconsConfiguration = iconsConfiguration;
    }
    public void ShowProducts(ItemCost[] items)
    {
        foreach (ItemCost item in items)
        {
            ShopProductButton instance = Instantiate(_shopProductButtonPrefab, _productsContainer);
            instance.Name = item.Key.Value;
            instance.Icon = _iconsConfiguration[item.Key.Value];
            instance.Key = item.Key;
            instance.OnButtonClick.AddListener(ShowRequirements);
            instance.Construct();
            _productsList.Add(instance);
            _itemsCost.Add(item.Key.Value, item);
        }
    }
    public void ClearPanels()
    {
        _selectedProduct = null;
        _buyButton.interactable = false;
        _itemsCost.Clear();
        ClearList(_itemsList);
        ClearList(_productsList);
    }

    private void ClearList<T>(List<T> list) where T : MonoBehaviour
    {
        T[] array = list.ToArray();
        int count = array.Length;
        for (int i = 0; i < count; i++)
            Destroy(array[i].gameObject);
        list.Clear();
    }

    private void ShowRequirements(Key key)
    {
        _selectedProduct = key;
        ClearList(_itemsList);
        ItemCount[] items = _itemsCost[key.Value].Cost;
        foreach (ItemCount item in items)
        {
            RequirementItemPresenter instance = Instantiate(_requirementPrefab, _requirementsContainer);
            instance.Count = item.Count;
            instance.Icon = _iconsConfiguration[item.Key.Value];
            _itemsList.Add(instance);
        }
        _buyButton.interactable = CheckProductForEvaluable(key);
    }
    private bool CheckProductForEvaluable(Key key)
    {
        if (_playerInventorySystem.GetCount(key.Value) > 0 && _configurations.ItemsOptions.Singletone.Contains(key))
            return false;
        foreach (ItemCount itemCount in _itemsCost[key.Value].Cost)
        {
            if (_playerInventorySystem.GetCount(itemCount.Key.Value) < itemCount.Count)
                return false;
        }
        return true;
    }
    private void OnBuyButtonPressed()
    {
        _playerInventorySystem.AddItem(_selectedProduct.Value, 1);
        foreach (ItemCount item in _itemsCost[_selectedProduct.Value].Cost)
            _playerInventorySystem.RemoveItem(item.Key.Value, item.Count);
        _buyButton.interactable = CheckProductForEvaluable(_selectedProduct);
    }
}
