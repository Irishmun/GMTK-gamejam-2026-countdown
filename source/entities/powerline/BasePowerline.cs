using Godot;
using static Godot.WebSocketPeer;

public partial class BasePowerline : Node
{
    [Signal] public delegate void PowerStateChangedEventHandler(bool powered);

    [Export] private Node2D powerNode;

    private bool _powered;

    public override void _Ready()
    {
        if (powerNode != null)
        {
            powerNode.Visible = _powered;
        }
    }

    public void SetPoweredState(bool state)
    {
        _powered = state;
        if (powerNode != null)
        {
            powerNode.Visible = state;
        }
        EmitSignalPowerStateChanged(state);
    }
    public bool Powered { get => _powered; set => SetPoweredState(value); }
}
