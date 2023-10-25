using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

[Serializable]
public class InteractableComp
{
    [SerializeField] private Transform _iconTransform;
    [SerializeField] private float _interactTime;
    [SerializeField] private float _recoverTime;

    private readonly SEvent _onInteractionStart = new();
    private readonly SEvent _onInteractionCancel = new();
    private readonly SEvent _onInteractionPerformed = new();
    private readonly SEvent<bool> _onEnable = new();
    private readonly SEvent<bool> _onRecovered = new();
    private bool _isValid = true;
    private bool _isRecovered = true;
    private IAppInputSystem _appInputSystem;
    private bool _isEnable;
    private bool _isInteracting;

    public Transform IconTransform
        => _iconTransform;
    public bool IsValid
    {
        get => _isValid;
        set => _isValid = value;
    }
    public bool IsEnable
    {
        get => _isEnable;
        set
        {
            if (!_isRecovered)
                return;
            _isEnable = value;
            if (value)
            {
                if (_isValid)
                {
                    _appInputSystem.OnInteractionStarted.AddListener(OnInteractionStarted);
                    _appInputSystem.OnInteractionPerformed.AddListener(OnInteractionPerformed);
                    _appInputSystem.OnInteractionCanceled.AddListener(OnInteractionCanceled);
                    _appInputSystem.SetInteractionTime(_interactTime);
                }
                _onEnable.Invoke(true);
            }
            else
            {
                _onEnable.Invoke(false);
                _appInputSystem.OnInteractionStarted.ClearListeners();
                _appInputSystem.OnInteractionPerformed.ClearListeners();
                _appInputSystem.OnInteractionCanceled.ClearListeners();
            }
        }
    }
    public SEvent OnStarted
        => _onInteractionStart;
    public SEvent OnCancel
        => _onInteractionCancel;
    public SEvent OnPerformed
        => _onInteractionPerformed;
    public SEvent<bool> OnEnable
        => _onEnable;
    public SEvent<bool> OnRecovered
        => _onRecovered;
    public bool IsInteracting
        => _isInteracting;

    public void Construct(IAppInputSystem appInputSystem) 
        => _appInputSystem = appInputSystem;

    private void OnInteractionCanceled()
    {
        _isInteracting = false;
        _onInteractionCancel.Invoke();
    }
    private void OnInteractionPerformed()
    {
        _isInteracting = false;
        _onInteractionPerformed.Invoke();
        Recover()
            .Forget();
        IsEnable = false;
        _onEnable.Invoke(false);
        _isRecovered = false;
        _onRecovered.Invoke(false);
    }
    private void OnInteractionStarted()
    {
        _isInteracting = true;
        _onInteractionStart.Invoke();
    }
    private async UniTask Recover()
    {
        int delay = (int)(_recoverTime * 1000);
        await UniTask.Delay(delay);
        _isRecovered = true;
        _onRecovered.Invoke(true);
    }
}
