using Godot;

public partial class CollapsingBridge : Node2D
{
    private const string ANIM_COLLAPSE = "Collapse";

    [Export] private AnimationPlayer animationPlayer;
    [Export] private RoomTransitionCamera roomCamera;
    [Export] private Area2D playerOnBridge;
    [Export] private int collapseAtTimeLeft = 60;
    [Export] private int playerFallDistance = 10;

    private Node _oldPlayerParent;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        this.Visible = false;
        GlobalTime.Instance.TimeLeft += Instance_TimeLeft;
        SubScribe(false);
    }

    public override void _ExitTree()
    {
        GlobalTime.Instance.TimeLeft -= Instance_TimeLeft;
    }

    private void PlayerOnBridge_BodyEntered(Node2D body)
    {
        if (body is not PlayerMovement)
        { return; }
        this.CallDeferred("ReparentNode", body, playerOnBridge);
    }

    private void PlayerOnBridge_BodyExited(Node2D body)
    {
        if (body is not PlayerMovement)
        { return; }
        this.CallDeferred("ReparentNode", body, _oldPlayerParent);
    }

    private void Instance_TimeLeft(int time)
    {
        if (time <= collapseAtTimeLeft)
        {
            this.Visible = true;
            animationPlayer.Play(ANIM_COLLAPSE);
            GlobalTime.Instance.TimeLeft -= Instance_TimeLeft;
        }
    }

    private void ReparentNode(Node2D node, Node newParent)
    {
        UnSubScribe(true);
        Vector2 globalPos = node.GlobalPosition;
        _oldPlayerParent = node.GetParent();
        GD.Print("parent is null:" + _oldPlayerParent == null);
        _oldPlayerParent.RemoveChild(node);
        newParent.AddChild(node);
        node.GlobalPosition = globalPos;
        SubScribe(true);
    }

    private void UnSubScribe(bool includeRoomCam)
    {
        playerOnBridge.BodyEntered -= PlayerOnBridge_BodyEntered;
        playerOnBridge.BodyExited -= PlayerOnBridge_BodyExited;
        if (includeRoomCam)
        {
            roomCamera.UnSubscribeAll();
        }
    }

    private void SubScribe(bool includeRoomCam)
    {
        playerOnBridge.BodyEntered += PlayerOnBridge_BodyEntered;
        playerOnBridge.BodyExited += PlayerOnBridge_BodyExited;
        if (includeRoomCam)
        {
            roomCamera.SubscribeAll();
        }
    }
}
