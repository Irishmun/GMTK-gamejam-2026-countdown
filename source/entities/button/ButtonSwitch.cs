using Godot;
using System.Collections.Generic;

public partial class ButtonSwitch : Area2D
{
    [Signal] public delegate void ButtonChangedEventHandler(bool pressed);

    [Export] private Sprite2D sprite;
    [Export] private bool stayPressed = false;
    [Export] private int releasedFrame = 0;
    [Export] private int pressedFrame = 1;
    [Export] private AudioStreamPlayer2D audio;

    private List<Node> presentBodies;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        presentBodies = new List<Node>();
        sprite.Frame = releasedFrame;
        this.BodyEntered += ButtonSwitch_BodyEntered;
        this.BodyExited += ButtonSwitch_BodyExited;
    }
    private void ButtonSwitch_BodyEntered(Node2D body)
    {
        if (presentBodies.Contains(body))
        { return; }

        int oldCount = presentBodies.Count;

        presentBodies.Add(body);

        if (oldCount == 0)
        {
            audio.Play();
            EmitSignalButtonChanged(true);
            sprite.Frame = pressedFrame;
        }
    }

    private void ButtonSwitch_BodyExited(Node2D body)
    {
        if (!presentBodies.Contains(body) || stayPressed)
        { return; }


        presentBodies.Remove(body);

        if (presentBodies.Count == 0)
        {
            EmitSignalButtonChanged(false);
            sprite.Frame = releasedFrame;
        }
    }

}
