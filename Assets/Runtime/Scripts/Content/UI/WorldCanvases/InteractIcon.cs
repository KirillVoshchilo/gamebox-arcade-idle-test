using App.Logic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace App.Content.UI.WorldCanvases
{
    public sealed class InteractIcon : MonoBehaviour
    {
        [SerializeField] private GameObject _buttonTip;
        [SerializeField] private TextMeshProUGUI _pressEText;
        [SerializeField] private TextMeshProUGUI _holdEText;
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
        public bool HoldMode
        {
            set
            {
                if (value)
                {
                    _holdEText.gameObject.SetActive(true);
                    _pressEText.gameObject.SetActive(false);
                }
                else
                {
                    _holdEText.gameObject.SetActive(false);
                    _pressEText.gameObject.SetActive(true);
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
            => _progressImage.fillAmount = value;

        private async UniTask OrientProcess()
        {
            while (_isEnable)
            {
                transform.LookAt(transform.position + _mainCameraTransform.forward);
                await UniTask.Delay(100);
            }
        }
    }
}