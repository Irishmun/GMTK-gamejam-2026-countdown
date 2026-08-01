using Godot;
using System;
using System.Collections.Generic;

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
        _state = !_state;
        audio.Play();
        EmitSignalSwitchToggled(_state);
        sprite.Frame = _state? pressedFrame: releasedFrame;
    }
}
