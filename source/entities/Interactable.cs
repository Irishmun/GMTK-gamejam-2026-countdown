using Godot;

public partial class Interactable : Node2D
{
    [Export] public bool CanInteract { get; set; } = true;

    private Node _oldParent = null;

    public void ReparentMe(Node newParent, Vector2 newGlobalPosition)
    {
        //bool bounce = newParent == _oldParent;
        _oldParent = this.GetParent();
        _oldParent.RemoveChild(this);
        //GD.Print($"newParent: {newParent} | this: {this}");
        newParent.AddChild(this);
        //if (bounce)
        //{
        //
        //    Tween tween = GetTree().CreateTween();
        //    tween.SetEase(Tween.EaseType.Out);
        //    tween.SetTrans(Tween.TransitionType.Bounce);
        //    tween.TweenProperty(this, "global_position:y", newGlobalPosition.Y, 0.2f);
        //    tween.Parallel();
        //    tween.SetEase(Tween.EaseType.In);
        //    tween.SetTrans(Tween.TransitionType.Circ);
        //    tween.TweenProperty(this, "global_position:x", newGlobalPosition.X, 0.2f);
        //}
        //else
        //{
            this.GlobalPosition = newGlobalPosition;
        //}
    }

    public Node OldParent => _oldParent;
}
