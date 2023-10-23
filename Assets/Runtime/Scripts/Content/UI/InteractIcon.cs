using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public sealed class InteractIcon : MonoBehaviour
{
    [SerializeField] private GameObject _buttonTip;
    [SerializeField] private GameObject _progressBar;
    [SerializeField] private Image _progressImage;
    [SerializeField] private Canvas _canvas;

    private Transform _mainCameraTransform;
    private bool _isEnable;

    public bool IsEnable
    {
        get => _isEnable;
        set
        {
            _isEnable = value;
            if (value)
            {
                OrientProcess()
                        .Forget();
            }
        }
    }

    [Inject]
    public void Construct(CamerasStorage camerasStorage)
    {
        _canvas.worldCamera = camerasStorage.MainCamera;
        _mainCameraTransform = camerasStorage.MainCamera.transform;
    }

    public void SetPosition(Vector3 position)
        => transform.position = position;
    public void OpenTip()
        => _buttonTip.SetActive(true);
    public void OpenProgress()
        => _progressBar.SetActive(true);
    public void CloseTip()
        => _buttonTip.SetActive(false);
    public void CloseProgress()
        => _progressBar.SetActive(false);
    public void SetProgress(float value)
    {
        _progressImage.fillAmount = value;
        Debug.Log($"«аполнил шкалу прогресса на иконке на {value}");
    }
    private async UniTask OrientProcess()
    {
        while (_isEnable)
        {
            transform.LookAt(transform.position + _mainCameraTransform.forward);
            await UniTask.Delay(100);
        }
    }
}
