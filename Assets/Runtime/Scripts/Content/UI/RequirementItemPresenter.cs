using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RequirementItemPresenter : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private TextMeshProUGUI _itemCount;

    public Sprite Icon
    {
        get => _iconImage.sprite;
        set => _iconImage.sprite = value;
    }
    public int Count
    {
        get => Convert.ToInt32(_itemCount.text);
        set => _itemCount.text = value.ToString();
    }
}
