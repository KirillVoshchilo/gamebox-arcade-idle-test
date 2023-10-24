using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class MainMenuPresenter : MonoBehaviour
{
    [SerializeField] private Button _startGameButton;
    [SerializeField] private Button _closeAppButton;
    [SerializeField] private Button _descriptionButton;
    [SerializeField] private Button _closeDescriptionButton;
    [SerializeField] private GameObject _descriptionPanel;

    private IAppInputSystem _appInputSystem;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem)
    {
        appInputSystem.EscapeIsEnable = false;
        appInputSystem.PlayerMovingIsEnable = false;
        _appInputSystem = appInputSystem;
        _closeAppButton.onClick.AddListener(OnCloseAppClicked);
        _startGameButton.onClick.AddListener(OnStartNewGameButton);
        _closeDescriptionButton.onClick.AddListener(OnCloseDescriptionClicked);
        _descriptionButton.onClick.AddListener(OnDescriptionClicked);
    }

    private void OnDescriptionClicked()
        => _descriptionPanel.SetActive(true);
    private void OnCloseDescriptionClicked()
        => _descriptionPanel.SetActive(false);

    private void OnStartNewGameButton()
    {
        _appInputSystem.EscapeIsEnable = true;
        _appInputSystem.PlayerMovingIsEnable = true;
        gameObject.SetActive(false);
    }
    private void OnCloseAppClicked()
        => Application.Quit();
}
