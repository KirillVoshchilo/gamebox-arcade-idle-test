using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public sealed class InventoryPresenter : MonoBehaviour
{
    [SerializeField] private InventoryItemPresenter _prefab;
    [SerializeField] private Transform _content;

    private PlayerInventorySystem _playerInventorySystem;
    private IconsConfiguration _consConfiguration;
    private List<InventoryItemPresenter> _itemsList = new();

    [Inject]
    public void Construct(PlayerInventorySystem playerInventorySystem,
        IconsConfiguration iconsConfiguration)
    {
        _playerInventorySystem = playerInventorySystem;
        _consConfiguration = iconsConfiguration;
    }
    public void FillWithItems()
    {
        foreach ((string name, int count) item in _playerInventorySystem.AllItems)
        {
            InventoryItemPresenter presenter = Instantiate(_prefab, _content);
            presenter.Count = item.count;
            presenter.Name = item.name;
            presenter.Icon = _consConfiguration[item.name];
            _itemsList.Add(presenter);
        }
    }
    public void Clear()
    {
        InventoryItemPresenter[] array = _itemsList.ToArray();
        int count = array.Length;
        for (int i = 0; i < count; i++)
            Destroy(array[i].gameObject);
        _itemsList.Clear();
    }
}
