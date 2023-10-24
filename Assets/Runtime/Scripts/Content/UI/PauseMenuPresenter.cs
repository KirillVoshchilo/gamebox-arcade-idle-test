using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

public class PauseMenuPresenter : MonoBehaviour
{
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _closeAppButton;

    private IAppInputSystem _appInputSystem;

    [Inject]
    public void Construct(IAppInputSystem appInputSystem)
    {
        _appInputSystem = appInputSystem;
        _closeAppButton.onClick.AddListener(OnCloseAppClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
    }

    private void OnContinueClicked()
    {
        _appInputSystem.PlayerMovingIsEnable = true;
        gameObject.SetActive(false);
    }
    private void OnCloseAppClicked()
        => SceneManager.LoadScene(0, LoadSceneMode.Single);
}