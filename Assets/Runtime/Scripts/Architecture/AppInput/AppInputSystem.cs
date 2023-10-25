using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.InputSystem;

public sealed class AppInputSystem : GameBoxArcadeIdle.IPlayerActions, IAppInputSystem
{
    private readonly GameBoxArcadeIdle _interactions;
    private readonly SEvent<float> _onInteractionPercantagechanged = new();
    private readonly SEvent _onMovingStarted = new();
    private readonly SEvent _onMovingStoped = new();
    private readonly SEvent _onLookingStarted = new();
    private readonly SEvent _onLookingStoped = new();
    private readonly SEvent _onInteractionStarted = new();
    private readonly SEvent _onInteractionPerformed = new();
    private readonly SEvent _onInteractionCanceled = new();
    private readonly SEvent _onEscapePressed = new();
    private readonly SEvent _onInventoryPressed = new();
    private Vector2 _moveDirection;
    private Vector2 _lookDirection;
    private bool _isMoving;
    private bool _isLooking;
    private bool _escapeIsEnable = true;
    private bool _inventoryIsEnable = true;
    private bool _playerMovingIsEnable = true;

    public Vector2 MoveDirection
        => _moveDirection;
    public bool IsMoving
        => _isMoving;
    public float InteractionPercentage
        => _interactions.Player.Interact.GetTimeoutCompletionPercentage();
    public SEvent OnMovingStarted
        => _onMovingStarted;
    public SEvent OnMovingStoped
        => _onMovingStoped;
    public SEvent OnInteractionStarted
        => _onInteractionStarted;
    public SEvent OnInteractionPerformed
        => _onInteractionPerformed;
    public SEvent OnInteractionCanceled
        => _onInteractionCanceled;
    public SEvent OnEscapePressed
        => _onEscapePressed;
    public SEvent OnInventoryPressed
        => _onInventoryPressed;
    public SEvent<float> OnInteractionPercantagechanged
        => _onInteractionPercantagechanged;
    public bool EscapeIsEnable
    {
        get => _escapeIsEnable;
        set => _escapeIsEnable = value;
    }
    public bool InventoryIsEnable
    {
        get => _inventoryIsEnable;
        set => _inventoryIsEnable = value;
    }
    public bool PlayerMovingIsEnable
    {
        get => _playerMovingIsEnable;
        set => _playerMovingIsEnable = value;
    }

    public AppInputSystem()
    {
        _interactions = new GameBoxArcadeIdle();
        _interactions.Player.SetCallbacks(this);
        _interactions.Player.Enable();
    }

    public void SetInteractionTime(float duration)
        => _interactions.Player.Interact.ApplyParameterOverride(nameof(HoldInteraction.duration), duration);
    public void OnMove(InputAction.CallbackContext context)
    {
        if (!_playerMovingIsEnable)
            return;
        _moveDirection = context.ReadValue<Vector2>();
        if (_moveDirection != Vector2.zero && !_isMoving)
        {
            _isMoving = true;
            _onMovingStarted.Invoke();
        }
        if (_isMoving && _moveDirection == Vector2.zero)
        {
            _isMoving = false;
            _onMovingStoped.Invoke();
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!_playerMovingIsEnable)
            return;
        _onInteractionPercantagechanged.Invoke(_interactions.Player.Interact.GetTimeoutCompletionPercentage());
        if (context.phase == InputActionPhase.Canceled)
            _onInteractionCanceled.Invoke();
        if (context.phase == InputActionPhase.Started)
            _onInteractionStarted.Invoke();
        if (context.phase == InputActionPhase.Performed)
            _onInteractionPerformed.Invoke();
    }
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookDirection = context.ReadValue<Vector2>();
        if (_lookDirection != Vector2.zero && !_isMoving)
        {
            _isLooking = true;
            _onLookingStarted.Invoke();
        }
        if (_isLooking && _lookDirection == Vector2.zero)
        {
            _isLooking = false;
            _onLookingStoped.Invoke();
        }
    }
    public void OnEsc(InputAction.CallbackContext context)
    {
        if (!_escapeIsEnable)
            return;
        if (context.phase == InputActionPhase.Performed)
            _onEscapePressed.Invoke();
    }
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (!_inventoryIsEnable)
            return;
        if (context.phase == InputActionPhase.Performed)
            _onInventoryPressed.Invoke();
    }
}

public interface IAppInputSystem
{
    float InteractionPercentage { get; }
    bool IsMoving { get; }
    Vector2 MoveDirection { get; }
    SEvent OnEscapePressed { get; }
    SEvent OnInteractionCanceled { get; }
    SEvent OnInteractionPerformed { get; }
    SEvent OnInteractionStarted { get; }
    SEvent OnInventoryPressed { get; }
    SEvent OnMovingStarted { get; }
    SEvent OnMovingStoped { get; }
    SEvent<float> OnInteractionPercantagechanged { get; }
    bool EscapeIsEnable { get; set; }
    bool InventoryIsEnable { get; set; }
    bool PlayerMovingIsEnable { get; set; }

    void SetInteractionTime(float duration);
}