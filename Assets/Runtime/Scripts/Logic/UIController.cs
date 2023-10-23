using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public sealed class UIController : MonoBehaviour
{
    [SerializeField] private InventoryPresenter _inventoryPresenter;
    [SerializeField] private MainMenuPresenter _mainMenuPresenter;

    private IAppInputSystem _appInputSystem;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem)
    {
        _appInputSystem = appInputSystem;
        _appInputSystem.OnEscapePressed.AddListener(OnEscapePressed);
        _appInputSystem.OnInventoryPressed.AddListener(OnInventoryPressed);
    }

    private void OnEscapePressed()
    {
        if (_inventoryPresenter.gameObject.activeSelf)
        {
            CloseInventory();
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
        _inventoryPresenter.gameObject.SetActive(true);
        _inventoryPresenter.FillWithItems();
    }
}
