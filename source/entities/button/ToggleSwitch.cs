using Godot;

public partial class ToggleSwitch : Area2D
{
    [Signal] public delegate void SwitchToggledEventHandler(bool pressed);

    [Export] private Sprite2D sprite;
    [Export] private int releasedFrame = 0;
    [Export] private int pressedFrame = 1;
    [Export] private AudioStreamPlayer2D audio;

    private bool _state = false;

    public void Interact()
    {
        SetVisuals(!_state);
    }

    public void SetVisuals(bool enabled)
    {
        if (_state == enabled)
        {
            return;
        }

        _state = enabled;
        audio.Play();
        EmitSignalSwitchToggled(_state);
        sprite.Frame = _state ? pressedFrame : releasedFrame;
    }

    public bool State { get => _state; set => SetVisuals(value); }
}
