using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public sealed class UIController : MonoBehaviour
{
    [SerializeField] private InventoryPresenter _inventoryPresenter;
    [SerializeField] private MainMenuPresenter _mainMenuPresenter;
    [SerializeField] private ShopPresenter _shopPresenter;

    private IAppInputSystem _appInputSystem;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem)
    {
        _appInputSystem = appInputSystem;
        _appInputSystem.OnEscapePressed.AddListener(OnEscapePressed);
        _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
    }
    public void OpenShop(ItemCost[] products)
    {
        _appInputSystem.EscapeIsEnable = true;
        _appInputSystem.PlayerMovingIsEnable = false;
        _shopPresenter.gameObject.SetActive(true);
        _shopPresenter.ShowProducts(products);
    }
    public void CloseShop()
    {
        _appInputSystem.PlayerMovingIsEnable = true;
        _appInputSystem.EscapeIsEnable = false;
        _shopPresenter.ClearPanels();
        _shopPresenter.gameObject.SetActive(false);
    }

    private void OnEscapePressed()
    {
        Debug.Log("Это надо переписать");
        if (_inventoryPresenter.gameObject.activeSelf)
        {
            CloseInventory();
            _appInputSystem.PlayerMovingIsEnable = true;
            return;
        }
        if (_shopPresenter.gameObject.activeSelf)
        {
            CloseShop();
            _appInputSystem.PlayerMovingIsEnable = true;
            return;
        }
    }
    private void OnInventoryPressed()
    {
        if (_inventoryPresenter.gameObject.activeSelf)
        {
            CloseInventory();
            _appInputSystem.PlayerMovingIsEnable = true;
        }
        else
        {
            OpenInventory();
            _appInputSystem.PlayerMovingIsEnable = false;
        }
    }
    private void CloseInventory()
    {
        _inventoryPresenter.Clear();
        _inventoryPresenter.gameObject.SetActive(false);
    }
    private void OpenInventory()
    {
        _appInputSystem.EscapeIsEnable = true;
        _inventoryPresenter.gameObject.SetActive(true);
        _inventoryPresenter.FillWithItems();
    }
}
