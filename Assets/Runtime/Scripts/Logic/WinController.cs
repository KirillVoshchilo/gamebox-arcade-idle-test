using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class WinController : MonoBehaviour, IDestructable
{
    [SerializeField] private ShopFactory _shopFactory;
    [SerializeField] private ShopEntity _shopEntity;

    private UIController _uiController;

    [Inject]
    public void Construct(UIController uIController)
    {
        Debug.Log("Условие победы создано");
        _uiController = uIController;
        _shopFactory.OnCreated.AddListener(OnFactoryCreated);
    }
    public void Destruct()
    {
        _shopFactory.OnCreated.RemoveListener(OnFactoryCreated);
    }

    private void OnFactoryCreated(ShopEntity entity)
    {
        if (entity.gameObject.name == _shopEntity.gameObject.name)
            _uiController.ShowWinCanvas();
    }
}
