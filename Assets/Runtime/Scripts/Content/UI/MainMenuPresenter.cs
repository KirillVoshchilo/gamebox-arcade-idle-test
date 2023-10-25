using Cysharp.Threading.Tasks;
using System;
using System.ComponentModel;
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
    private LevelLoaderSystem _levelLoader;
    private Configuration _configuration;
    private PlayerInventorySystem _playerInventorySystem;
    private PlayerEntity _playerEntity;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem,
        LevelLoaderSystem levelLoader,
        PlayerEntity playerEntity,
        PlayerInventorySystem playerInventorySystem,
        Configuration configuration)
    {
        _configuration = configuration;
        _playerInventorySystem = playerInventorySystem;
        _playerEntity = playerEntity;
        _levelLoader = levelLoader;
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
        _levelLoader.LoadScene(LevelLoaderSystem.FIRST_LEVEL, OnCompleteLoading)
            .Forget();
        int count = _configuration.StartInventoryConfiguration.Items.Length;
        for (int i = 0; i < count; i++)
        {
            string name = _configuration.StartInventoryConfiguration.Items[i].Key.Value;
            int quantity = _configuration.StartInventoryConfiguration.Items[i].Count;
            _playerInventorySystem.AddItem(name, quantity);
        }
        _playerEntity.GetComponent<Rigidbody>().useGravity = true;
        _appInputSystem.EscapeIsEnable = true;
        _appInputSystem.InventoryIsEnable = true;
        _appInputSystem.PlayerMovingIsEnable = true;
        gameObject.SetActive(false);
    }

    private void OnCompleteLoading(LevelStorage storage)
    {
        _playerEntity.transform.position = storage.PlayerTransform.position;
    }
    private void OnCloseAppClicked()
        => Application.Quit();
}
